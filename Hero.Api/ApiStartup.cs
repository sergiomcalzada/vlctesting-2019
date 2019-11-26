using System;
using Hero.Api.Configuration.Authorization;
using Hero.Api.Middleware.ExceptionMiddleware;
using Hero.Business.Repository;
using Hero.Business.Service;
using Hero.Domain;
using Hero.Domain.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hero.Api
{
    public static class ApiStartup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void AddApi(this IServiceCollection services, IConfiguration config, Action<DbContextOptionsBuilder> configureDataContext)
        {
            // Add AspNetCore framework services.
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(setup =>
                {
                    // configure mvc options
                })
                .AddApplicationPart(typeof(ApiStartup).Assembly)
                .AddControllersAsServices();

            services.AddApiAuthorization();
            services.AddExceptionMiddleware();

            services.AddDbContext<DataContext>(configureDataContext);
            services.AddScoped<IHeroService, HeroService>();
            services.AddScoped<IEntityRepository, EntityRepository>();
            services.AddScoped<IQueryableInvoker, EfQueryableInvoker>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void UseApi(this IApplicationBuilder app, Action<IEndpointRouteBuilder> configureEndpoints = default)
        {

            app.UseExceptionMiddleware();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                configureEndpoints?.Invoke(endpoints);
            });

        }
    }
}