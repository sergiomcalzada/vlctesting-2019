using System;
using System.Collections.Generic;
using System.IO;
using Hero.Api.Configuration.Authorization;
using Hero.Api.Host.Options.OAuth2;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hero.Api.Host.Options.Swagger
{
    public class SwaggerGenOptionsConfigure : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly OAuth2Options oauth2Options;
        private ApiAuthorizationOptions apiAuthorizationOptions;

        public SwaggerGenOptionsConfigure(
            IOptions<OAuth2Options> oauth2Options,
            IOptions<ApiAuthorizationOptions> apiAuthorizationOptions
            )
        {
            this.oauth2Options = oauth2Options.Value;
            this.apiAuthorizationOptions = apiAuthorizationOptions.Value;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Swashbuckle Sample API",
                    Description = "A sample API for testing Swashbuckle"
                }
            );

            if (this.apiAuthorizationOptions.IsEnabled)
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{this.oauth2Options.Authority}/connect/authorize"),
                            Scopes = new Dictionary<string, string>
                            {
                                //{"openid","openid"},
                                {$"{this.oauth2Options.Scope}", this.oauth2Options.Scope}
                            }
                        }
                    }
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            }



            options.CustomOperationIds(description =>
            {
                if (description.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    var controllerName = controllerActionDescriptor.ControllerName;
                    var actionName = controllerActionDescriptor.AttributeRouteInfo.Name ??
                                     controllerActionDescriptor.ActionName;
                    var actionTemplate = controllerActionDescriptor.AttributeRouteInfo.Template;
                    var actionVerb = description.HttpMethod;
                    return $"{controllerName}_{actionName}_{actionTemplate}_{actionVerb}";
                }

                return description.ActionDescriptor.Id;
            });
            options.OperationFilter<BadRequestOperationFilter>();

            options.IncludeXmlComments(this.GetXmlCommentsPath(PlatformServices.Default.Application));
        }

        private string GetXmlCommentsPath(Microsoft.Extensions.PlatformAbstractions.ApplicationEnvironment appEnvironment)
        {
            return Path.Combine(appEnvironment.ApplicationBasePath, "Hero.Api.Host.xml");
        }
    }
}