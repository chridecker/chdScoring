using chdScoring.BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text;

namespace chdScoring.Main.WebServer.Extensions
{
    public class ApiLoggingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, IApiLogger logger)
        {
            var ip = context.Connection.RemoteIpAddress;
            var hostName = string.Empty;
            if (ip != null)
            {
                var hostEntry = await Dns.GetHostEntryAsync(ip);
                hostName = hostEntry.HostName;
            }
            var apiKey = context.Request.Headers.TryGetValue("x-api-key", out var key) ? key.ToString() : string.Empty;
            var call =
                $"{(!string.IsNullOrWhiteSpace(hostName) ? hostName : ip)}({apiKey}) {context.Request.Method} {context.Request.Path}{context.Request.QueryString}";
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
