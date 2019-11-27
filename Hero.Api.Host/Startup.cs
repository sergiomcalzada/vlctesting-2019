using HealthChecks.UI.Client;
using Hero.Api.Host.Options;
using Hero.Api.Host.Options.ApplicationInsights;
using Hero.Api.Host.Options.OAuth2;
using Hero.Api.Host.Options.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Hero.Api.Host
{
    public class Startup
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            this.environment = env;
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApi(this.configuration, opt =>
             {
                 opt.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection"));
             });

            services.AddCors();
            services.AddSwagger();
            services.AddOAuth2();
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "ready" });
            services.AddHealthChecksUI();
            services.AddApplicationInsights(this.configuration);

            //Add Redis if configured
            services.AddRedis(this.configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod()
            );

            //ConfigureJwt Swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseApi(endpoints =>
            {
                endpoints.MapHealthChecks("/healthz/ready", new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/healthz/live", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecksUI(setup =>
                {
                    setup.AddCustomStylesheet("wwwroot\\css\\healthz.css");
                });
            });
            

        }
    }
}
