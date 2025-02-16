namespace CC_Model.Converter
{
    public class CurrencyConverterResponse
    {
        public string Base { get; set; }
        public decimal Amount { get; set; }
        public List<ConvertionRate> Rates { get; set; }
    }

    public class ConvertionRate
    {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public decimal ConvertedAmount { get; set; }
    }
}