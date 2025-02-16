namespace CC_UnitTest.TestModel
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Data? Data { get; set; }
    }

    public class Data
    {
        public string Token { get; set; }
    }
}