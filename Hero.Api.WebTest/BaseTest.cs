using Hero.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Hero.Api.WebTest
{
    public class BaseTest : IClassFixture<CompositionRootFixture>
    {
        protected readonly CompositionRootFixture Fixture;
        public TestServer Server { get; }

        public BaseTest(CompositionRootFixture fixture, ITestOutputHelper outputHelper)
        {
            this.Fixture = fixture;
            var builder = this.CreateHostBuilder(outputHelper);



            this.Server = new TestServer(builder);

            using (var scope = this.Server.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>())
                {
                    
                    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    var isEnabled = configuration.GetValue("Redis:IsEnabled", false);
                    var redisConfig = configuration.GetValue("Redis:Configuration", "localhost");
                    outputHelper.WriteLine($"Enabled ({isEnabled}) on {redisConfig}", isEnabled, redisConfig);

                    var conn_string = configuration.GetConnectionString("DefaultConnection");
                    outputHelper.WriteLine($"conn_string ({conn_string})", conn_string);


                    dbContext.Database.EnsureCreated();
                    dbContext.Database.Migrate();
                    dbContext.Seed();
                }
            }
        }

        private IWebHostBuilder CreateHostBuilder(ITestOutputHelper outputHelper)
        {
            return new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config
                        .SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", false)
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging
                        .AddConfiguration(hostingContext.Configuration.GetSection("Logging"))
                        .AddConsole()
                        .AddDebug();
                    logging.AddProvider(new TestLoggerProvider(outputHelper));
                })
                .ConfigureKestrel(serverOptions =>
                        {
                            // Tune kestrel options
                        })
                .UseStartup<Startup>();
        }


    }
}