namespace CC_Services.ExchangeRate
{
    using CC_Model.Converter;
    using CC_Model.ExchangeRateVM;

    public interface IRealTimeCurrencyConverter
    {
        Task<(CurrencyConverterResponse, string)> ConvertAmount(CurrencyConvert request);

        Task<(CurrencyConverterResponse, string)> ConvertAmount(CurrencyConvertLimitSymbol request);
    }
}