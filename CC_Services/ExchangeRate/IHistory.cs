namespace CC_Services.ExchangeRate
{
    using CC_Model.ExchangeRateVM;

    public interface IHistory
    {
        Task<object> GetExchangeRateHistory(ExchangeRateHistoryReq request);
    }
}