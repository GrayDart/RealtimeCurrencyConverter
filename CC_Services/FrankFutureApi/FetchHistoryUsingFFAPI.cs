namespace CC_Services.FrankFutureApi
{
    using AppSettings_Reader;
    using CC_Model;
    using CC_Model.ExchangeRateVM;
    using CC_Model.ProviderVM;
    using CC_Services.ExchangeRate;
    using CC_Services.ThirdPartyAPI;

    public class FetchHistoryUsingFFAPI : BaseServices, IHistory, IHistoryBaseCurrency, IHistoryBaseAndTargetCurrency
    {
        private readonly Helper _appSettingHelper;

        private const string _appSettingKey = "FrankFutureAPI";
        private readonly FrankFutureAPI _frankFutureAPI;
        private readonly IThirdPartyAPICallServices _thirdPartyAPICallServices;
        public string baseUrl = string.Empty;
        public string apiUrl = string.Empty;
        public string method = "GET";
        public string cacheKey = string.Empty;
        public int requestedPage = 1;
        public int requestedPageSize = 10;

        public FetchHistoryUsingFFAPI(IThirdPartyAPICallServices thirdPartyAPICallServices)
        {
            _appSettingHelper = new Helper();
            _frankFutureAPI = _appSettingHelper.GetSettingsBySection<FrankFutureAPI>(_appSettingKey);

            if (_frankFutureAPI == null)
            {
                throw new Exception("FrankFuture API settings missing in the AppSetting");
            }

            baseUrl = _frankFutureAPI.BaseUrl;
            _thirdPartyAPICallServices = thirdPartyAPICallServices;
        }

        /// <summary>
        /// Get Exchange history with filter (from date, to date, currency and limit currency) with pagination
        /// </summary>
        /// <param name="request">from date, to date, currency and limit currency </param>
        /// <returns>paginated record</returns>
        public async Task<object> GetExchangeRateHistory(ExchangeRateHistoryWithBaseAndTargetCurrency request)
        {
            apiUrl = _frankFutureAPI.HistoryByRangeAndBaseCurrencySymbol;

            apiUrl = apiUrl.Replace("[FDATE]", $"{request.FromDate:yyyy-MM-dd}")
                            .Replace("[TDATE]", $"{request.ToDate:yyyy-MM-dd}")
                            .Replace("[CURRENCY]", request.BaseCurrency)
                            .Replace("[SYMBOL]", string.Join(',', request.TargetCurrency));

            requestedPage = request.CurrentPage;
            requestedPageSize = request.PageSize;
            cacheKey = $"{request.FromDate:yyyyMMdd}{request.ToDate:yyyyMMdd}{request.BaseCurrency}";

            return await APIInterface();
        }

        /// <summary>
        /// Get Exchange history with filter (from date, to date, currency with pagination
        /// </summary>
        /// <param name="request">from date, to date, currency</param>
        /// <returns>paginated record</returns>
        public async Task<object> GetExchangeRateHistory(ExchangeRateHistoryWithBaseCurrencyReq request)
        {
            apiUrl = _frankFutureAPI.HistoryByRangeAndCurrency;

            apiUrl = apiUrl.Replace("[FDATE]", $"{request.FromDate:yyyy-MM-dd}")
                            .Replace("[TDATE]", $"{request.ToDate:yyyy-MM-dd}")
                            .Replace("[CURRENCY]", request.BaseCurrency);

            requestedPage = request.CurrentPage;
            requestedPageSize = request.PageSize;
            cacheKey = $"{request.FromDate:yyyyMMdd}{request.ToDate:yyyyMMdd}{request.BaseCurrency}";

            return await APIInterface();
        }

        /// <summary>
        /// Get Exchange history with filter (from date, to date  with pagination
        /// </summary>
        /// <param name="request">from date, to date limit currency </param>
        /// <returns>paginated record</returns>
        public async Task<object> GetExchangeRateHistory(ExchangeRateHistoryReq request)
        {
            apiUrl = _frankFutureAPI.HistoryByRange;

            apiUrl = apiUrl.Replace("[FDATE]", $"{request.FromDate:yyyy-MM-dd}").Replace("[TDATE]", $"{request.ToDate:yyyy-MM-dd}");

            requestedPage = request.CurrentPage;
            requestedPageSize = request.PageSize;

            cacheKey = $"{request.FromDate:yyyyMMdd}{request.ToDate:yyyyMMdd}";

            return await APIInterface();
        }

        #region private function applicable only on this class

        /// <summary>
        /// API Interface with pagination
        /// </summary>
        /// <returns></returns>
        private async Task<object> APIInterface()
        {
            PaginatedResult<KeyValuePair<string, Dictionary<string, decimal>>> result = new();
            var apiResponse = await _thirdPartyAPICallServices.CallAPIAsync<ExchangeRateHistoryResponse>(baseUrl, apiUrl, cacheKey, method);

            if (apiResponse != null && apiResponse.Rates?.Count > 0)
            {
                requestedPage = requestedPage > 0 ? requestedPage : 1;
                requestedPageSize = requestedPageSize > 0 ? requestedPageSize : 10;

                var paginatedData = GetPaginatedData(apiResponse.Rates, requestedPage, requestedPageSize);

                if (paginatedData != null)
                {
                    result = paginatedData;
                    result.Start_date = apiResponse.Start_date;
                    result.End_date = apiResponse.End_date;
                    result.Amount = apiResponse.Amount;
                    result.Base = apiResponse.Base;
                }
            }

            return result;
        }

        #endregion private function applicable only on this class
    }
}