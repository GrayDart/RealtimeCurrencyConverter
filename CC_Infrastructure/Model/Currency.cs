namespace CC_Infrastructure.Model
{
    public partial class Currency
    {
        public int ID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
