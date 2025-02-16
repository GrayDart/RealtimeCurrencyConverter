namespace CC_Model.ProviderVM
{
    public class FrankFutureAPI
    {
        public string BaseUrl { get; set; }
        public string Latest { get; set; }
        public string LatestBase { get; set; }
        public string LatestSymbol { get; set; }
        public string LatestBaseAndSymbol { get; set; }
        public string HistoryByDate { get; set; }
        public string HistoryByDateTillLatest { get; set; }
        public string HistoryByDateTillLatestAndSymbol { get; set; }
        public string HistoryByRange { get; set; }
        public string HistoryByRangeAndSybol { get; set; }
        public string HistoryByRangeAndCurrency { get; set; }
        public string HistoryByRangeAndBaseCurrencySymbol { get; set; }
    }


}