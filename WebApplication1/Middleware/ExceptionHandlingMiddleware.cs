using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace App.WEB.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошло исключение: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest, exception.Message),
                InvalidOperationException => (StatusCodes.Status400BadRequest, exception.Message),
                ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
                NotImplementedException => (StatusCodes.Status400BadRequest, exception.Message),
                DbUpdateException => (StatusCodes.Status400BadRequest, exception.InnerException == null? exception.Message : exception.InnerException.Message),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, exception.Message),
                SecurityTokenException => (StatusCodes.Status401Unauthorized, exception.Message),
                KeyNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsJsonAsync(new
            {
                StatusCode = statusCode,
                Message = message,
                Details = exception.Message //statusCode == 500 ? null : exception.Message
            });
        }
    }
}
