using App.BLL.DTO;
using App.BLL.Interfaces;
using App.DAL.Entities;
using App.DAL.Infrastructure;
using App.DAL.Interfaces;
using App.WEB;
using App.WEB.BLL.Infrastructure;
using App.WEB.BLL.Interfaces;
using App.WEB.BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}) 
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuerSigningKey = true
        };
    })
    .AddCookie("Cookies", options => // ��������� ����� �������������� ����� ����
    {
        options.Cookie.HttpOnly = true; // ���� ���������� ����� JavaScript
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // ���� ���������� ������ �� HTTPS
        options.Cookie.SameSite = SameSiteMode.Strict; // ������ �� CSRF
        //options.LoginPath = "/Account/Login"; // ���� ��� ��������������� �� �������� �����
    });
builder.Services.Configure<CookieSettings>(builder.Configuration.GetSection("CookieSettings"));
builder.Services.AddAuthorization();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher, RBKDF2PasswordHasher>();
builder.Services.AddScoped<IAuthService<Passenger>, PassengerAuthService>();
builder.Services.AddScoped<IAuthService<Carrier>, CarrierAuthService>();
builder.Services.AddScoped<IAuthService<Driver>, DriverAuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();   // ���������� middleware ����������� 
app.MapControllers();
app.UseDefaultFiles();  // ��������� ������� html �� ���������
app.UseStaticFiles();
//app.MapFallbackToFile("/index.html");



app.Run();
