using FUNewsManagementSystem.Models.Common;
using System.Net;
using System.Text.Json;

namespace FUNewsManagementSystem.Middleware
{
    /// <summary>
    /// Global exception handler middleware for centralized error handling
    /// </summary>
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
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
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = exception switch
            {
                ArgumentNullException => new
                {
                    statusCode = (int)HttpStatusCode.BadRequest,
                    errors = new Dictionary<string, string[]>
                    {
                        { "request", new[] { exception.Message } }
                    }
                },
                UnauthorizedAccessException => new
                {
                    statusCode = (int)HttpStatusCode.Unauthorized,
                    errors = new Dictionary<string, string[]>
                    {
                        { "authentication", new[] { "Unauthorized access" } }
                    }
                },
                KeyNotFoundException => new
                {
                    statusCode = (int)HttpStatusCode.NotFound,
                    errors = new Dictionary<string, string[]>
                    {
                        { "resource", new[] { exception.Message } }
                    }
                },
                _ => new
                {
                    statusCode = (int)HttpStatusCode.InternalServerError,
                    errors = new Dictionary<string, string[]>
                    {
                        { "server", new[] { "An internal server error occurred. Please try again later." } }
                    }
                }
            };

            context.Response.StatusCode = response.statusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(new { errors = response.errors }, jsonOptions)
            );
        }
    }

    public static class GlobalExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandler>();
        }
    }
}
