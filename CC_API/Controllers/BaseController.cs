/********************************************************
/// <summary>
///   Namespace        :      Controllers
///   Class            :      BaseController
///   Description      :      Base features for all the controllers
///   Author           :      Subash R (GrayDart)                   Date: Feb, 2025
///   Notes            :      -Nil-
///   Revision History:
///   Name:           Date:        Description:
/// </summary>
******************************************************/
namespace CC_API.Controllers
{
    using CC_Model;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// API response declaration
        /// </summary>
        public Result? response;

        public InternalReq _internalReq;

        /// <summary>
        /// Session expired
        /// </summary>
        private const string SessionErrorMsg = "invalid_session_or_expired";

        /// <summary>
        /// This method helps to construct the return response
        /// </summary>
        /// <param name="statusCode">api status code</param>
        /// <param name="message">request message</param>
        /// <param name="responseData">response object or data</param>
        /// <returns>API result Object</returns>
        internal static Result ConstructReturnResponse(int statusCode, string message, object? responseData = null)
        {
            return new Result
            {
                StatusCode = statusCode,
                Message = message,
                Data = responseData!
            };
        }

        /// <summary>
        /// Format API response data with status code and message
        /// </summary>
        /// <param name="errMsg">error message from service</param>
        /// <param name="httpMethod">http method (GET, POST, PUT, DELETE)</param>
        /// <param name="responseData">Response data</param>
        /// <param name="successMsg"></param>
        /// <returns>API result Object</returns>
        internal Result FormatResponse(string errMsg, Enums.HttpMethods httpMethod, object? responseData = null, string successMsg = "")
        {
            var result = new Result();

            try
            {
                if (!string.IsNullOrEmpty(errMsg))
                {
                    // can implement language based message return

                    if (errMsg == SessionErrorMsg)
                    {
                        result = ConstructReturnResponse(103, errMsg); // means session expired, must login again
                    }
                    else
                    {
                        result = ConstructReturnResponse(104, errMsg);
                    }
                }
                else if (responseData != null)
                {
                    switch (httpMethod)
                    {
                        case Enums.HttpMethods.GET:
                            if (successMsg?.Length == 0)
                            {
                                successMsg = "Action Succeeded";
                            }
                            break;

                        case Enums.HttpMethods.POST:
                            if (successMsg?.Length == 0)
                            {
                                successMsg = "Record created successfully";
                            }
                            break;
                    }

                    switch (httpMethod)
                    {
                        case Enums.HttpMethods.GET:
                            result = ConstructReturnResponse(100, successMsg!, responseData);
                            break;

                        case Enums.HttpMethods.POST:
                            result = ConstructReturnResponse(101, successMsg!, responseData);
                            break;
                    }
                }
                else
                {
                    result = ConstructReturnResponse(105, "no results found");
                }
            }
            catch
            {
                result = ConstructReturnResponse(105, "Error while processing your request");
            }

            return result;
        }

        /// <summary>
        /// Read request necessary information
        /// </summary>
        /// <param name="context">request context</param>
        /// <returns>internal request detial</returns>
        internal InternalReq ReadInternalReq(HttpRequest? context)
        {
            // read claims we can use advance option to read claims or store and get the session detail from Radis Cache
            var identifierID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"); // I have used INT (it supposed to be GUID in production)
            _internalReq = new InternalReq
            {
                UserID = identifierID,
                Name = User.FindFirstValue(ClaimTypes.Name) ?? "",
                Role = User.FindFirstValue(ClaimTypes.Role) ?? "",
                TrackingInfo = new TrackingInfo
                {
                    IPAddress = GetIP(context?.HttpContext),
                    GeoLocation = ""// can implement api call here to get the geo location based on the IP address
                },
                Lang = GetLang(context?.HttpContext),
                VirtualPath = "./",
                IsValidUser = identifierID > 0,
                Message = identifierID > 0 ? "" : "Invalid token or expired"
            };

            return _internalReq;
        }

        /// <summary>
        /// Get client ip using http context
        /// </summary>
        /// <param name="requestContext">http context</param>
        /// <returns>ip address</returns>
        internal static string GetIP(HttpContext? requestContext)
        {
            string? ip;
            if (!string.IsNullOrEmpty(requestContext?.Request.Headers["X-Forwarded-For"]))
            {
                ip = requestContext.Request.Headers["X-Forwarded-For"];
            }
            else
            {
                ip = requestContext?.Request.HttpContext.Features?.Get<IHttpConnectionFeature>().RemoteIpAddress?.ToString();
            }
            if (string.IsNullOrEmpty(ip) || ip == "::1")
            {
                ip = requestContext?.Connection.RemoteIpAddress?.ToString();
            }

            return ip ?? "";
        }

        internal static string GetLang(HttpContext? requestContext)
        {
            string lang = "en";
            try
            {
                string? userLang = requestContext?.Request?.Headers["lang"] ?? "";

                if (!string.IsNullOrEmpty(userLang))
                {
                    lang = userLang;
                }
            }
            catch
            {
            }

            return lang;
        }
    }
}