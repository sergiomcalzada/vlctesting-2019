using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hero.Api.Host.Options.OAuth2
{
    public static class OAuth2ServiceCollectionExtensions
    {
        public static void AddOAuth2(this IServiceCollection services)
        {
            services.AddOptions<OAuth2Options>()
                .Configure<IConfiguration>((settings, config) =>
                    {
                        config.GetSection(OAuth2Options.Section).Bind(settings);
                    });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();
            services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsConfigure>();
        }


    }
}