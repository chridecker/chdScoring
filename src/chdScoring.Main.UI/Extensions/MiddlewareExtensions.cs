using Microsoft.AspNetCore.Builder;

namespace chdScoring.Main.UI.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder AddApiLogger(this IApplicationBuilder app)
        => app.UseMiddleware<ApiLoggingMiddleware>();

    }
}
