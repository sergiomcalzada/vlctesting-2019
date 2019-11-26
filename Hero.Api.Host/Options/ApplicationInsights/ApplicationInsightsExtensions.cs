using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Hero.Api.Host.Options.ApplicationInsights
{
    public static class ApplicationInsightsExtensions
    {
        public static void AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetApplicationInsightsOptions();
            if (options.IsEnabled)
            {
                services.AddApplicationInsightsTelemetry();
            }


        }

        public static void ConfigureApplicationInsightsLogging(this LoggerConfiguration loggerConfiguration, IConfiguration configuration)
        {
            var options = configuration.GetApplicationInsightsOptions();
            if (options.IsEnabled)
            {
                loggerConfiguration.WriteTo.ApplicationInsights(TelemetryConverter.Traces);
            }


        }

        public static ApplicationInsightsOptions GetApplicationInsightsOptions(this IConfiguration configuration)
        {
            return configuration
                       .GetSection(ApplicationInsightsOptions.Section)
                       .Get<ApplicationInsightsOptions>() ?? new ApplicationInsightsOptions();
        }
    }
}
