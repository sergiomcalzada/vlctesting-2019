using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hero.Api.WebTest
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration config)
        {
            var isEnabled = config.GetValue("Redis:IsEnabled", false);
            if (isEnabled)
            {
                var redisConfig = config.GetValue("Redis:Configuration", "localhost");

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConfig;
                });
            }
        }
    }
}
