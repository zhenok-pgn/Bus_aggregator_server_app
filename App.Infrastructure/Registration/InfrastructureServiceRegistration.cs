using App.Application.Services;
using App.Core.Interfaces;
using App.Infrastructure.Data;
using App.Infrastructure.Identity;
using App.Infrastructure.Security;
using App.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace App.Infrastructure.Registration
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = "Host=localhost;Port=5432;Database=postgres;Username=zhenya;Password=";

            // Регистрируем DbContext в DI контейнере
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ValidateIssuerSigningKey = true
                    };
                })
                .AddCookie("Cookies", options => // Добавляем схему аутентификации через куки
                {
                    options.Cookie.HttpOnly = true; // Кука недоступна через JavaScript
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Кука передается только по HTTPS
                    options.Cookie.SameSite = SameSiteMode.Strict; // Защита от CSRF
                    //options.LoginPath = "/Account/Login"; // Путь для перенаправления на страницу входа
                });
            services.AddAuthorization();

            services.Configure<CookieSettings>(configuration.GetSection("CookieSettings"));

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IRouteScheduleService, RouteScheduleService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<ITariffService, TariffService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITripService, TripService>();

            // Auto Mapper Configurations
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}