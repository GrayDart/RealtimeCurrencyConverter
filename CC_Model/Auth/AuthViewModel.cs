namespace CC_Model.Auth
{
    using System.ComponentModel.DataAnnotations;

    public class LoginReq
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginRes
    {
        public string Token { get; set; }
    }
}