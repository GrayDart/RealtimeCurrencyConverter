namespace CC_Services.Logger
{
    using CC_Model;
    using Microsoft.Extensions.Logging;

    public class EndpointLogger : IEndpointLogger
    {
        private readonly ILogger<EndpointLogger> _logger;

        public EndpointLogger(ILogger<EndpointLogger> logger) => _logger = logger;

        #region log before

        public void LogBegin(InternalReq internalReq, string loggingBase, string endpoint)
        {
            _logger.LogInformation("{0} Client ID:{1} ", loggingBase, internalReq.UserID);
            _logger.LogInformation("{0} IP Address:{1} ", loggingBase, internalReq.TrackingInfo.IPAddress);
            _logger.LogInformation("{0} EndPoint:{1} ", loggingBase, endpoint);
        }

        public void LogEnd(string loggingBase, int status, string message)
        {
            _logger.LogInformation("{0} Response Status:{1} ", loggingBase, status);
            _logger.LogInformation("{0} Response Message:{1} ", loggingBase, message);
        }

        #endregion log before
    }
}