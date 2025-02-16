namespace CC_Services.FrankFutureApi
{
    using AppSettings_Reader;
    using CC_Model.ExchangeRateVM;
    using CC_Model.ProviderVM;
    using CC_Services.ExchangeRate;
    using CC_Services.ThirdPartyAPI;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class FetchLatestUsingFFAPI : BaseServices, ILatestExchangeRate
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="thirdPartyAPICallServices">Injection</param>
        /// <exception cref="Exception"></exception>
        public FetchLatestUsingFFAPI(IThirdPartyAPICallServices thirdPartyAPICallServices)
        {
            _appSettingHelper = new Helper();
            _frankFutureAPI = _appSettingHelper.GetSettingsBySection<FrankFutureAPI>(_appSettingKey);

            if (_frankFutureAPI == null)
            {
                throw new Exception("FrankFuture API setting missing in the AppSetting");
            }

            baseUrl = _frankFutureAPI.BaseUrl;
            _thirdPartyAPICallServices = thirdPartyAPICallServices;
        }

        #region API Services

        /// <summary>
        /// Get latest exchange rate with base currency
        /// </summary>
        /// <param name="baseCurrency">base currency (default EUR)</param>
        /// <returns></returns>
        public async Task<ExchangeRateResponse> GetLatestExchangeRate(string baseCurrency)
        {
            apiUrl = _frankFutureAPI.LatestBase;

            apiUrl = apiUrl.Replace("[CURRENCY]", baseCurrency);

            cacheKey = $"latest_{baseCurrency}";

            return await APIInterface();
        }

        /// <summary>
        /// Get latest exchange rate with base currency and limit symbols
        /// </summary>
        /// <param name="baseCurrency">base currency</param>
        /// <param name="symbols">limit symbols list</param>
        /// <returns></returns>
        public async Task<ExchangeRateResponse> GetLatestExchangeRate(string baseCurrency, List<string> symbols)
        {
            apiUrl = _frankFutureAPI.LatestBaseAndSymbol;

            var strSymbols = string.Join(',', symbols);
            apiUrl = apiUrl.Replace("[CURRENCY]", baseCurrency)
                            .Replace("[SYMBOL]", strSymbols);

            cacheKey = $"latest_{baseCurrency}{strSymbols}";

            return await APIInterface();
        }

        #endregion API Services

        #region private function applicable only on this class

        /// <summary>
        /// API interface
        /// </summary>
        /// <returns></returns>
        private async Task<ExchangeRateResponse> APIInterface()
        {
            ExchangeRateResponse result = new();
            var apiResponse = await _thirdPartyAPICallServices.CallAPIAsync<ExchangeRateResponse>(baseUrl, apiUrl, cacheKey, method);

            if (apiResponse != null)
            {
                result = apiResponse;
            }

            return result;
        }

        #endregion private function applicable only on this class
    }
}