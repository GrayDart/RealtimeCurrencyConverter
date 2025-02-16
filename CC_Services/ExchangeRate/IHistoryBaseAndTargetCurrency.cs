namespace CC_Services.ExchangeRate
{
    using CC_Model.ExchangeRateVM;

    public interface IHistoryBaseAndTargetCurrency
    {
        Task<object> GetExchangeRateHistory(ExchangeRateHistoryWithBaseAndTargetCurrency request);
    }
}