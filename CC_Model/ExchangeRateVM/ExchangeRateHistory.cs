namespace CC_Model.ExchangeRateVM
{
    public class ExchangeRateHistoryReq : PagingRequest
    {
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
    }

    public class ExchangeRateHistoryWithBaseCurrencyReq : ExchangeRateHistoryReq
    {
        public string BaseCurrency { get; set; }
    }

    public class ExchangeRateHistoryWithBaseAndTargetCurrency : ExchangeRateHistoryWithBaseCurrencyReq
    {
        public List<string> TargetCurrency { get; set; }
    }
}