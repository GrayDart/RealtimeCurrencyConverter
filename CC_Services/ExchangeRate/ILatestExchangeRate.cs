namespace CC_Services.ExchangeRate
{
    using CC_Model.ExchangeRateVM;

    public interface ILatestExchangeRate
    {
        Task<ExchangeRateResponse> GetLatestExchangeRate(string baseCurrency);

        Task<ExchangeRateResponse> GetLatestExchangeRate(string baseCurrency, List<string> symbols);
    }
}