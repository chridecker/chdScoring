using chdScoring.BusinessLogic.Services;
using Microsoft.AspNetCore.Http;

namespace chdScoring.Main.WebServer.Extensions
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public ApiLoggingMiddleware(RequestDelegate next) => this._next = next;


        public async Task InvokeAsync(HttpContext context, IApiLogger logger)
        {
            var time = DateTime.Now;
            await logger.Log($"{time}: {context.Request.Host} {context.Request.Method} {context.Request.Path}");
            await this._next(context);
        }
    }
}
