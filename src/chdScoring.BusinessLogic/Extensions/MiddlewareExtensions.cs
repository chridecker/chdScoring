using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder AddApiLogger(this IApplicationBuilder app)
        => app.UseMiddleware<ApiLoggingMiddleware>();

    }
}
