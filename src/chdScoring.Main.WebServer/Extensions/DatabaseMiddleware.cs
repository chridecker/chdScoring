using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.Main.WebServer.Extensions
{
    public class DatabaseMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, IInterceptedDatabaseConfiguration config)
        {
            if (context.Request.Query.ContainsKey("db"))
            {
                config.Name = context.Request.Query["db"];
            }

            await next(context);
        }
    }
}
