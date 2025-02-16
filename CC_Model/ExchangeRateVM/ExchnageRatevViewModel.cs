namespace CC_Model.ExchangeRateVM
{
    public class ExchangeRateResponse
    {
        public string Base { get; set; } = "EUR";
        public string Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public class CurrencyConvert
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
    }

    public class CurrencyConvertLimitSymbol : CurrencyConvert
    {
        public List<string> Symbols { get; set; }
    }

    public class HistoryResponse
    {
        public double Amount { get; set; }
        public string Base { get; set; }
        public string Start_date { get; set; }
        public string End_date { get; set; }
    }

    public class ExchangeRateHistoryResponse : HistoryResponse
    {
        public Dictionary<string,Dictionary<string,decimal>> Rates { get; set; }
    }
}