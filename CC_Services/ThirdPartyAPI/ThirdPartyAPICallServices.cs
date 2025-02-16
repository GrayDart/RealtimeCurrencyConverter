namespace CC_Services.ThirdPartyAPI
{
    using Microsoft.Extensions.Caching.Memory;
    using Newtonsoft.Json;
    using Polly;
    using Polly.CircuitBreaker;
    using Polly.Contrib.WaitAndRetry;
    using Polly.Extensions.Http;
    using System.Net;
    using System.Net.Http;

    public class ThirdPartyAPICallServices : BaseServices, IThirdPartyAPICallServices
    {
        // we can do this common place (program.cs)
        private readonly IAsyncPolicy<HttpResponseMessage> _asyncPolicy;

        // we can do this on common plage (program.cs)
        private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreaker;

        // we have to use Redis or any NoSQL options for best cloud based cashing
        private readonly IMemoryCache _memoryCache;

        private int _cachingDuragionSeconds = 10;
        private int _retryCount = 5;

        public ThirdPartyAPICallServices(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _appsettinghelper = new();

            _cachingDuragionSeconds = _appsettinghelper.GetAppSettingsKeyValue<int>("Caching_Duration_Seconds");
            _retryCount = _appsettinghelper.GetAppSettingsKeyValue<int>("Retry_Count");

            _asyncPolicy = Policy<HttpResponseMessage>
                                .Handle<HttpRequestException>()
                                .OrResult(x => x.StatusCode >= HttpStatusCode.InternalServerError || x.StatusCode == HttpStatusCode.RequestTimeout)
                                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), _retryCount));

            _circuitBreaker = Policy<HttpResponseMessage>
                    .Handle<HttpRequestException>()
                    .OrTransientHttpError()
                    .AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(20)
                        , 10, TimeSpan.FromSeconds(15));
        }

        public async Task<TResponse> CallAPIAsync<TResponse>(string baseUrl, string apiUrl, string cacheKey, string method)
        {
            TResponse? result = default;
            HttpResponseMessage? httpReq = null;

            //// check record exists in Memory cash (minimizing direct call to api)
            var hasCashed = CheckMemoryCash<TResponse>(cacheKey);
            if (hasCashed != null)
            {
                return hasCashed;
            }

            //// check circuit breaker
            var circuitBraker = CheckCircuitBraker<TResponse>(cacheKey);
            if (circuitBraker != null)
            {
                return circuitBraker;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                switch (method)
                {
                    case "GET":
                        httpReq = await _circuitBreaker.ExecuteAsync(async () => await _asyncPolicy.ExecuteAsync(async () => await client.GetAsync(apiUrl)));
                        break;
                }
            }

            switch (httpReq?.StatusCode)
            {
                case HttpStatusCode.OK:
                    var resultContent = await httpReq.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<TResponse>(resultContent);

                    _memoryCache.Set(cacheKey, result, TimeSpan.FromSeconds(_cachingDuragionSeconds));
                    break;

                case HttpStatusCode.BadRequest:
                    break;

                case HttpStatusCode.NotFound:
                    break;

                default:
                    break;
            }

            return result;
        }

        private TResponse CheckMemoryCash<TResponse>(string cacheKey)
        {
            TResponse? result = default;

            var cacheddata = _memoryCache.Get<TResponse>(cacheKey);

            if (cacheddata != null)
            {
                result = cacheddata;
            }

            return result;
        }

        private TResponse CheckCircuitBraker<TResponse>(string cacheKey)
        {
            TResponse? result = default;

            if (_circuitBreaker.CircuitState is CircuitState.Open or CircuitState.Isolated)
            {
                result = _memoryCache.Get<TResponse>(cacheKey);
            }

            return result;
        }
    }
}