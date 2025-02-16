namespace CC_Services.Auth
{
    using CC_Infrastructure.Data;
    using CC_Model.Auth;
    using JWT_TokenProvider;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;

    public class AuthenticationServices : BaseServices, IAuthenticationServices
    {
        private TokenProvider _tokenProvider;

        public AuthenticationServices(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
            _appsettinghelper = new();
            _tokenProvider = new TokenProvider(_appsettinghelper.GetSettingsBySectionAndKey("JWTConfig", "JWTConfig"),
                                                        _appsettinghelper.GetSettingsBySectionAndKey("JWTConfig", "Audience"),
                                                        _appsettinghelper.GetSettingsBySectionAndKey("JWTConfig", "Secret"));
        }

        /// <summary>
        /// Authenticate User using Email and Password
        /// </summary>
        /// <param name="loginReq">login request object</param>
        /// <returns>return token</returns>
        public async Task<(LoginRes, string)> AuthenticateUser(LoginReq loginReq)
        {
            LoginRes result = new();
            var errMsg = string.Empty;

            // note (password should be hased to prevent plain text comparing)
            var isUserExist = await _dbcontext.Users.AsNoTracking().FirstOrDefaultAsync(t => t.Email == loginReq.Email && t.Password == loginReq.Password);

            if (isUserExist != null)
            {
                var token = _tokenProvider.GetToken(
                    [
                    new(ClaimTypes.NameIdentifier, isUserExist.ID.ToString()),
                    new(ClaimTypes.Name,isUserExist.Name),
                    new(ClaimTypes.Role,isUserExist.Role)
                    ], null, _appsettinghelper.GetSettingsBySectionAndKey<int>("JWTConfig", "AuthTokenTimeOut"));

                if (token != null)
                {
                    result.Token = token.AccessToken;

                    //token.RefreshToken use this token to refresh the access token
                }
                else
                {
                    errMsg = "Unable to generate token, opease try again";// must be implemented the language based reply (with keyvalue pair)
                }
            }
            else
            {
                errMsg = "Invalid username or password. Please check your credentials and try again.";
            }

            return (result, errMsg);
        }
    }
}