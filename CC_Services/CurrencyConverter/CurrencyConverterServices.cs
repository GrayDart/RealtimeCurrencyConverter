namespace CC_Services.CurrencyConverter
{
    using CC_Model.Converter;
    using CC_Model.ExchangeRateVM;
    using CC_Services.ExchangeRate;

    public class CurrencyConverterServices : BaseServices, IRealTimeCurrencyConverter
    {
        private readonly ILatestExchangeRate _latestExchangeRate;
        private List<string> _excludedCurrencies = new();

        public CurrencyConverterServices(ILatestExchangeRate latestExchangeRate)
        {
            _latestExchangeRate = latestExchangeRate;
            _appsettinghelper = new();

            _excludedCurrencies = _appsettinghelper.GetAppSettingsKeyValue<List<string>>("ExcludedCurrency");
        }

        /// <summary>
        /// Gets latest exchange rate and convert the given amount
        /// </summary>
        /// <param name="request">amount (decimal) and base currency</param>
        /// <returns></returns>
        public async Task<(CurrencyConverterResponse, string)> ConvertAmount(CurrencyConvert request)
        {
            CurrencyConverterResponse result = null;
            var message = string.Empty;

            var latestER = await _latestExchangeRate.GetLatestExchangeRate(request.Base);

            if (latestER != null)
            {
                result = new()
                {
                    Base = request.Base,
                    Amount = request.Amount,
                    Rates = latestER.Rates.Where(t => !_excludedCurrencies.Contains(t.Key)).Select(t => new ConvertionRate
                    {
                        Currency = t.Key,
                        Rate = t.Value,
                        ConvertedAmount = t.Value * request.Amount
                    }).ToList()
                };
            }

            return (result, message);
        }

        /// <summary>
        /// Gets latest exchange rate and convert the given amount and limit currency
        /// </summary>
        /// <param name="request">amount (decimal) and base and limit currency</param>
        /// <returns></returns>
        public async Task<(CurrencyConverterResponse, string)> ConvertAmount(CurrencyConvertLimitSymbol request)
        {
            CurrencyConverterResponse result = null;
            var message = string.Empty;

            if (IsExcludedCurrencyInvolved(request.Symbols))
            {
                message = "bad request";
            }
            else
            {
                var latestER = await _latestExchangeRate.GetLatestExchangeRate(request.Base, request.Symbols);

                if (latestER != null)
                {
                    result = new()
                    {
                        Base = request.Base,
                        Amount = request.Amount,
                        Rates = latestER.Rates.Where(t => !_excludedCurrencies.Contains(t.Key)).Select(t => new ConvertionRate
                        {
                            Currency = t.Key,
                            Rate = t.Value,
                            ConvertedAmount = t.Value * request.Amount
                        }).ToList()
                    };
                }
            }

            return (result, message);
        }

        private bool IsExcludedCurrencyInvolved(List<string> symbols)
        {
            var joinedString = string.Join(',', symbols);

            return _excludedCurrencies.Any(t => joinedString.Contains(t));
        }
    }
}