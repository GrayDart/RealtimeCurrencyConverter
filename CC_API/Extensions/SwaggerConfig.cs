/********************************************************
/// <summary>
///   Namespace        :      Extensions
///   Class            :      SwaggerConfig
///   Description      :      Swagger Gen Configuration extension
///   Author           :      Subash R (GrayDart)                   Date: Feb, 2025
///   Notes            :      -Nil-
///   Revision History:
///   Name:           Date:        Description:
/// </summary>
******************************************************/

namespace CC_API.Extensions
{
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.OpenApi.Models;
    using System.Reflection;

    /// <summary>
    /// Swagger Config statick class
    /// </summary>
    public static class SwaggerConfig
    {
        public static void AddSwaggerGen(this IServiceCollection swagger)
        {
            swagger.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CC API V1.0",
                    Description = "API documentation for version 1"
                });

                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return [api.GroupName];
                    }

                    var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                    if (controllerActionDescriptor != null)
                    {
                        return [controllerActionDescriptor.ControllerName];
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.DocInclusionPredicate((_, _) => true);
                c.IncludeXmlComments(xmlPath);
                c.IncludeXmlComments("./CC_API.xml");

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. " +
                                    "\r\n\r\nEnter 'Bearer' [space] and then your token in the text input below." +
                                    "\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, new string[] {}
                    }
                });
            });
        }
    }
}