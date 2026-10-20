using System.Text;
using chdScoring.BusinessLogic.Services;
using Microsoft.AspNetCore.Http;

namespace chdScoring.Main.WebServer.Extensions
{
    public class ApiLoggingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, IApiLogger logger)
        {
            var apiKey = context.Request.Headers.TryGetValue("x-api-key", out var key) ? key.ToString() : string.Empty;
            var call =
                $"{context.Request.Host}({apiKey}) {context.Request.Method} {context.Request.Path}{context.Request.QueryString}";
            var detail = string.Empty;
            if (logger.LogDetail)
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false, leaveOpen: true);
                detail = await reader.ReadToEndAsync(context.RequestAborted);
                context.Request.Body.Position = 0;
            }

            await logger.Log(call, detail);
            await next(context);
        }
    }
}
