using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Hero.Api.Host.Options.Swagger
{
    internal static class SwaggerServiceCollectionExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddOptions<SwaggerOAuth2Options>()
                .Configure<IConfiguration>((settings, config) =>
                {
                    config.GetSection(SwaggerOAuth2Options.Section).Bind(settings);
                });

            services.AddOptions<SwaggerOptions>();
            services.AddSingleton<IConfigureOptions<SwaggerOptions>, SwaggerOptionsConfigure>();

            services.AddOptions<SwaggerGenOptions>();
            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, SwaggerGenOptionsConfigure>();
            services.AddSwaggerGen();

            services.AddOptions<SwaggerUIOptions>();
            services.AddSingleton<IConfigureOptions<SwaggerUIOptions>, SwaggerUIOptionsConfigure>();
            
        }

    }
}