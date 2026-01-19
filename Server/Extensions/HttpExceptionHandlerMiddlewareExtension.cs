using Server.Middleware;

namespace Server.Extensions
{
    public static class HttpExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseHttpExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpExceptionHandlerMiddleware>();
        }
    }
}
