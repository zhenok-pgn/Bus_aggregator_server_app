using App.Infrastructure.Registration;
using App.WEB.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();   // добавление middleware авторизации 
app.MapControllers();

app.MapFallbackToFile("/passenger/{*path}", "passenger/index.html");
app.MapFallbackToFile("/carrier/{*path}", "carrier/index.html");
app.MapFallbackToFile("/driver/{*path}", "driver/index.html");

app.Run();
