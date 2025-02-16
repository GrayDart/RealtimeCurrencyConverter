/********************************************************
/// <summary>
///   Namespace        :      Controllers
///   Class            :      Converter AI controller
///   Description      :      Converter related API goes here
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
    using CC_Model.ExchangeRateVM;
    using CC_Services.ExchangeRate;
    using CC_Services.Logger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ConvertController : BaseController
    {
        #region Fields

        private readonly ILogger<ConvertController> _logger;
        private readonly IEndpointLogger _endpointLogger;
        private readonly IRealTimeCurrencyConverter _realTimeCurrencyConverter;

        private const string _loggingBase = "CONVERT API Log # ";

        #endregion Fields

        #region Constructor

        public ConvertController(ILogger<ConvertController> logger, IRealTimeCurrencyConverter realTimeCurrencyConverter, IEndpointLogger endpointLogger)
        {
            _logger = logger;
            _endpointLogger = endpointLogger;
            _realTimeCurrencyConverter = realTimeCurrencyConverter;
        }

        #endregion Constructor

        #region API

        /// <summary>
        /// Convert given amounts between different currencies
        /// </summary>
        /// <param name="request">Amount, base currency</param>
        /// <returns>Conversion rate</returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Staff")]
        [Route("", Name = "GetConversion")]
        public async Task<IActionResult> GetConversion(CurrencyConvert request)
        {
            try
            {
                ReadInternalReq(Request);

                _endpointLogger.LogBegin(_internalReq, _loggingBase, this.ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? "");

                (var svcResponse, string message) = await _realTimeCurrencyConverter.ConvertAmount(request);

                response = FormatResponse(message, Enums.HttpMethods.GET, svcResponse);

                _endpointLogger.LogEnd(_loggingBase, response.StatusCode, response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0} Invalid or unable to process your request", _loggingBase);
                response = ConstructReturnResponse(104, "{0} Invalid or unable to process your request", _loggingBase);
            }

            return Ok(response);
        }

        /// <summary>
        /// Convert amounts between different currencies with limit (symbols)
        /// </summary>
        /// <param name="request">Amount, base currency</param>
        /// <returns>Conversion rate</returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Staff")]
        [Route("limit", Name = "GetConversionByLimit")]
        public async Task<IActionResult> GetConversionByBaseLimitSymbol(CurrencyConvertLimitSymbol request)
        {
            try
            {
                ReadInternalReq(Request);

                _endpointLogger.LogBegin(_internalReq, _loggingBase, this.ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? "");

                (var svcResponse, string message) = await _realTimeCurrencyConverter.ConvertAmount(request);

                response = FormatResponse(message, Enums.HttpMethods.GET, svcResponse);

                _endpointLogger.LogEnd(_loggingBase, response.StatusCode, response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0} Invalid or unable to process your request", _loggingBase);
                response = ConstructReturnResponse(104, "{0} Invalid or unable to process your request", _loggingBase);
            }

            return Ok(response);
        }

        #endregion API
    }
}