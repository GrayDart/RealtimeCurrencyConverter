namespace CC_UnitTest.TestModel
{
    public class CurrencyConvert
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
    }

    public class CurrencyConvertLimitSymbol : CurrencyConvert
    {
        public List<string> Symbols { get; set; }
    }

    public class ConverterResonse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }
}