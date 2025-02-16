/********************************************************
/// <summary>
///   Namespace        :      Controllers
///   Class            :      History Controller
///   Description      :      History related API goes here
///   Author           :      Subash R (GrayDart)                   Date: Feb, 2025
///   Notes            :      -Nil-
///   Revision History:
///   Name:           Date:        Description:
/// </summary>
******************************************************/
namespace CC_API.Controllers
{
    using CC_Model.ExchangeRateVM;
    using CC_Model;
    using CC_Services.ExchangeRate;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Asp.Versioning;
    using CC_Services.Logger;

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class History : BaseController
    {
        #region Fields

        private readonly ILogger<History> _logger;
        private readonly IHistory _history;
        private readonly IEndpointLogger _endpointLogger;
        private readonly IHistoryBaseCurrency _historyBaseCurrency;
        private readonly IHistoryBaseAndTargetCurrency _historyBaseAndTargetCurrency;

        private const string _loggingBase = "HISTORY API Log # ";

        #endregion Fields

        #region Constructor

        public History(ILogger<History> logger,
                       IHistory history,
                       IEndpointLogger endpointLogger,
                       IHistoryBaseCurrency historyBaseCurrency,

                       IHistoryBaseAndTargetCurrency historyBaseAndTargetCurrency)
        {
            _logger = logger;
            _history = history;
            _endpointLogger = endpointLogger;
            _historyBaseCurrency = historyBaseCurrency;
            _historyBaseAndTargetCurrency = historyBaseAndTargetCurrency;
        }

        #endregion Constructor

        #region API

        /// <summary>
        /// Get history by range (from date and to date)
        /// </summary>
        /// <param name="request">from date and to date</param>
        /// <returns>history</returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Staff")]
        [Route("range", Name = "GetByRange")]
        public async Task<IActionResult> GetByRange(ExchangeRateHistoryReq request)
        {
            try
            {
                ReadInternalReq(Request);

                _endpointLogger.LogBegin(_internalReq, _loggingBase, this.ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? "");

                var svcResponse = await _history.GetExchangeRateHistory(request);

                response = FormatResponse("", Enums.HttpMethods.GET, svcResponse);

                _endpointLogger.LogEnd(_loggingBase, response.StatusCode, response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0} Invalid or unable to process your request",_loggingBase);
                response = ConstructReturnResponse(104, "{0} Invalid or unable to process your request", _loggingBase);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get history by range (from date and to date) with base currency
        /// </summary>
        /// <param name="request">from date, to date, base currency </param>
        /// <returns>history</returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [Route("rangewithbase", Name = "GetByRangeByBaseCurrency")]
        public async Task<IActionResult> GetByRangeByBaseCurrency(ExchangeRateHistoryWithBaseCurrencyReq request)
        {
            try
            {
                ReadInternalReq(Request);

                _endpointLogger.LogBegin(_internalReq, _loggingBase, this.ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? "");

                var svcResponse = await _historyBaseCurrency.GetExchangeRateHistory(request);

                response = FormatResponse("", Enums.HttpMethods.GET, svcResponse);

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
        /// Get history by range (from date and to date) with base currency
        /// </summary>
        /// <param name="request">from date, to date, base currency </param>
        /// <returns>history</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("rangewithlimit", Name = "GetByRangeByBaseCurrencyWithLimit")]
        public async Task<IActionResult> GetByRangeByBaseTargetCurrency(ExchangeRateHistoryWithBaseAndTargetCurrency request)
        {
            try
            {
                ReadInternalReq(Request);

                _endpointLogger.LogBegin(_internalReq, _loggingBase, this.ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? "");

                var svcResponse = await _historyBaseAndTargetCurrency.GetExchangeRateHistory(request);

                response = FormatResponse("", Enums.HttpMethods.GET, svcResponse);

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