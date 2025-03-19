using App.Application.Interfaces.Services;
using App.Application.Services;
using App.Core.Entities;
using App.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace App.Application.Registration
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRouteScheduleService, RouteScheduleService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IRouteStopService, RouteStopService>();
            services.AddScoped<ITariffService, TariffService>();

            return services;
        }
    }
}