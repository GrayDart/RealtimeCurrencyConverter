/********************************************************
/// <summary>
///   Namespace        :      Extensions
///   Class            :      Dependencies
///   Description      :      Extension for Dependency injection
///   Author           :      Subash R (GrayDart)                   Date: Feb, 2025
///   Notes            :      -Nil-
///   Revision History:
///   Name:           Date:        Description:
/// </summary>
******************************************************/

namespace CC_API.Extensions
{
    using CC_Services.Auth;
    using CC_Services.CurrencyConverter;
    using CC_Services.ExchangeRate;
    using CC_Services.FrankFutureApi;
    using CC_Services.Logger;
    using CC_Services.ThirdPartyAPI;
    using System.Net.Http;

    public static class Dependencies
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IEndpointLogger, EndpointLogger>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<IRealTimeCurrencyConverter, CurrencyConverterServices>();
            services.AddScoped<IThirdPartyAPICallServices, ThirdPartyAPICallServices>();
            services.AddScoped<ILatestExchangeRate, FetchLatestUsingFFAPI>();
            services.AddScoped<IHistoryBaseCurrency, FetchHistoryUsingFFAPI>();
            services.AddScoped<IHistoryBaseAndTargetCurrency, FetchHistoryUsingFFAPI>();
            services.AddScoped<IHistory, FetchHistoryUsingFFAPI>();

            services.AddHttpClient<IHttpClientFactory>();
        }
    }
}