using System.Net;
using System.Text.Json;

namespace BrainWave.WebUI.Middlewares
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Description);
    public class ExceptionHandlingMiddleware:IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)=>_logger = logger;

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }catch(Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }

        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unexpected error occurred.");    

            ExceptionResponse response = exception switch
            {
                ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, "Application exception occurred."),
                KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, "The request key not found."),
                UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized."),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please retry later.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
