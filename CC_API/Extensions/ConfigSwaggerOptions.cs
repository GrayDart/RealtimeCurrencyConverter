/********************************************************
/// <summary>
///   Namespace        :      Extensions
///   Class            :      ConfigSwaggerOptions
///   Description      :      Extension for swagger options Configuration
///   Author           :      Subash R (GrayDart)                   Date: Feb, 2025
///   Notes            :      -Nil-
///   Revision History:
///   Name:           Date:        Description:
/// </summary>
******************************************************/
namespace CC_API.Extensions
{
    using Asp.Versioning.ApiExplorer;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ConfigSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {

            //foreach (var description in _provider.ApiVersionDescriptions)
            //{
            //    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            //}
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description) => new()
        {
            Title = "BC CC API",
            Version = description.ApiVersion.ToString(),
            Description = "Currency Converter API"
        };
    }
}