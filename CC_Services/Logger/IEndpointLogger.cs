namespace CC_Services.Logger
{
    using CC_Model;

    public interface IEndpointLogger
    {
        void LogBegin(InternalReq internalReq, string loggingBase, string endpoint);

        void LogEnd(string loggingBase, int status, string message);
    }
}