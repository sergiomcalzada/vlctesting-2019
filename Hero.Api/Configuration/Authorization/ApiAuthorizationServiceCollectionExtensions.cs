using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hero.Api.Configuration.Authorization
{
    public static class ApiAuthorizationServiceCollectionExtensions
    {
        public static void AddApiAuthorization(this IServiceCollection services)
        {
            services.AddOptions<ApiAuthorizationOptions>()
                .Configure<IConfiguration>((settings, config) =>
                    {
                        config.GetSection(ApiAuthorizationOptions.Section).Bind(settings);
                    });

            services.AddAuthorization();
            services.AddSingleton<IConfigureOptions<AuthorizationOptions>, AuthorizationOptionsConfigure>();

        }


    }
}