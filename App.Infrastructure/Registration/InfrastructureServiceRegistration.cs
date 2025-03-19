using App.Application.Interfaces;
using App.Application.Interfaces.Repositories;
using App.Application.Interfaces.Services;
using App.Core.Entities;
using App.Core.Interfaces;
using App.Infrastructure.Data;
using App.Infrastructure.Identity;
using App.Infrastructure.Repositories;
using App.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace App.Infrastructure.Registration
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = "Host=localhost;Port=5432;Database=postgres;Username=zhenya;Password=";

            // Регистрируем DbContext в DI контейнере
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseNpgsql(connectionString)
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

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IRouteScheduleRepository, RouteScheduleRepository>();
            services.AddScoped<IRouteStopRepository, RouteStopRepository>();
            services.AddScoped<ITariffRepository, TariffRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, RBKDF2PasswordHasher>();

            services.AddScoped<IAuthService<Passenger>, PassengerAuthService>();
            services.AddScoped<IAuthService<Carrier>, CarrierAuthService>();
            services.AddScoped<IAuthService<Driver>, DriverAuthService>();

            return services;
        }
    }
}