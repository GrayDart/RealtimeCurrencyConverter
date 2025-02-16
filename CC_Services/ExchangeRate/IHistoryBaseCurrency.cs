namespace CC_Services.ExchangeRate
{
    using CC_Model.ExchangeRateVM;

    public interface IHistoryBaseCurrency
    {
        Task<object> GetExchangeRateHistory(ExchangeRateHistoryWithBaseCurrencyReq request);
    }
}