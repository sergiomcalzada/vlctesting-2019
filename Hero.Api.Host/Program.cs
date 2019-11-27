using System;
using System.IO;
using System.Threading.Tasks;
using Hero.Api.Host.Options.ApplicationInsights;
using Hero.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Hero.Api.Host
{
    public class Program
    {
        private const string OutputTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}]{NewLine}{Message:lj} {NewLine}{Exception}";
        public static async Task Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: OutputTemplate)
                .WriteTo.Debug(outputTemplate: OutputTemplate)
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    //var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                    //await dbContext.Database.EnsureCreatedAsync();

                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var isEnabled = configuration.GetValue("Redis:IsEnabled", false);
                    var redisConfig = configuration.GetValue("Redis:Configuration", "localhost");
                    Log.Logger.Information("Enabled (){isEnabled} on {config}", isEnabled, redisConfig);

                }


                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config
                        .SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                        .AddJsonFile("secrets/appsettings.json", true, true)
                        .AddEnvironmentVariables()
                        .AddCommandLine(args);
                    if (env.IsDevelopment())
                    {
                        config.AddUserSecrets<Startup>();
                    }

                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext();
                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        loggerConfiguration
                            .WriteTo.Debug(outputTemplate: OutputTemplate)
                            .WriteTo.Console(outputTemplate: OutputTemplate);
                    }

                    loggerConfiguration.ConfigureApplicationInsightsLogging(hostingContext.Configuration);

                }, true)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(serverOptions =>
                        {
                            // Tune kestrel options
                        })
                        .UseStartup<Startup>();
                });
        }
    }
}
