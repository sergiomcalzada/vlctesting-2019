using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hero.Api.Middleware.ExceptionMiddleware
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExceptionMiddleware(this IServiceCollection services)
        {
            services.AddOptions<ExceptionMiddlewareOptions>()
                .Configure<IConfiguration>((settings, config) =>
                {
                    config.GetSection(ExceptionMiddlewareOptions.Section).Bind(settings);
                });
            return services.AddSingleton<ExceptionMiddleware>();
        }

        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}