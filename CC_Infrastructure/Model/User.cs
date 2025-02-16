namespace CC_Infrastructure.Model
{
    public partial class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public int IsActive { get; set; } = 1;
    }
}