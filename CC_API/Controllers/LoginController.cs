/********************************************************
/// <summary>
///   Namespace        :      Controllers
///   Class            :      Login Controller
///   Description      :      Login related api goes here
///   Author           :      Subash R (GrayDart)                   Date: Feb, 2025
///   Notes            :      -Nil-
///   Revision History:
///   Name:           Date:        Description:
/// </summary>
******************************************************/
namespace CC_API.Controllers
{
    using Asp.Versioning;
    using CC_Model;
    using CC_Model.Auth;
    using CC_Services.Auth;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class LoginController : BaseController
    {
        #region Fields

        private readonly ILogger<LoginController> _logger;
        private readonly IAuthenticationServices _authenticationServices;

        #endregion Fields

        #region Constructor

        public LoginController(ILogger<LoginController> logger, IAuthenticationServices authenticationServices)
        {
            _logger = logger;
            _authenticationServices = authenticationServices;
        }

        #endregion Constructor

        #region API

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request">required email and password</param>
        /// <returns>token</returns>
        [HttpPost]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        [Route("", Name = "Login")]
        public async Task<IActionResult> AuthenticateUser(LoginReq request)
        {
            try
            {
                ReadInternalReq(Request);

                (var svcResponse, string message) = await _authenticationServices.AuthenticateUser(request);

                response = FormatResponse(message, Enums.HttpMethods.GET, svcResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid or unable to process your request");
                response = ConstructReturnResponse(104, "Invalid or unable to process your request");
            }

            return Ok(response);
        }

        #endregion API
    }
}