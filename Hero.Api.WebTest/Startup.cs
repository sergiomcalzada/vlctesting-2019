using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hero.Api.WebTest
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

            //Add test server authentication
            services.AddAuthentication(options =>
               {
                   options.DefaultScheme = TestServerDefaults.AuthenticationScheme;
               })
               .AddTestServer();

            //Add Redis if configured
            services.AddRedis(this.configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApi();
        }
    }
}
