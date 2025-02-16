namespace CC_Services.Auth
{
    using CC_Model.Auth;

    public interface IAuthenticationServices
    {
        Task<(LoginRes, string)> AuthenticateUser(LoginReq loginReq);
    }
}