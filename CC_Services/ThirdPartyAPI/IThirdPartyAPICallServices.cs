namespace CC_Services.ThirdPartyAPI
{
    public interface IThirdPartyAPICallServices
    {
        Task<TResponse> CallAPIAsync<TResponse>(string baseUrl, string apiUrl, string cacheKey, string method);
    }
}