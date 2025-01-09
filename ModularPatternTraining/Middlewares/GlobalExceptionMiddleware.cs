using Microsoft.Data.SqlClient;

namespace ModularPatternTraining.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var errorId = Guid.NewGuid();

            try
            {
                await _next(context);
            }
            
            catch (SqlException sqlEx)
            {
                var sqlErrorId = "SL-ERROR-00001";
                LogException(context, sqlEx, LogLevel.Error,errorId,sqlErrorId);
                await HandleExceptionAsync(context,"A database error occurred. Please try again later.", 500,errorId,sqlErrorId);
            }
            catch (Exception ex)
            {
                var generalErrorId = "GL-ERROR-00001";
                LogException(context, ex, LogLevel.Critical,errorId,generalErrorId);
                await HandleExceptionAsync(context, "An unexpected error occurred. Please contact support.", 500,errorId,generalErrorId);
            }
        }
        private string GetErrorId(HttpContext context)
        {
            
            return Guid.NewGuid().ToString();
        }
        private void LogException(HttpContext context, Exception ex, LogLevel logLevel,Guid errorId, string typeError)
        {
            _logger.Log(
                logLevel,
                ex,
                "Error ID: {ErrorId}| Error Type ID: {ErrorTypeId} |Exception occurred: {ExceptionType} | Message: {Message} | Path: {Path} | QueryString: {QueryString} | User: {User}",
                errorId,
                typeError,
                ex.GetType().Name,
                ex.Message,
                context.Request.Path,
                context.Request.QueryString,
                context.User?.Identity?.Name ?? "Anonymous"
            );
        }
        private static Task HandleExceptionAsync(HttpContext context, string message, int statusCode , Guid errorId, string typeError)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var result = new
            {
                StatusCode = statusCode,
                Message = message,
                ErrorId = errorId, 
                Path = context.Request.Path,
                Timestamp = DateTime.UtcNow
            };

            return context.Response.WriteAsJsonAsync(result);
        }
    }

}
