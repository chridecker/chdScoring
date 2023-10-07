using chdScoring.BusinessLogic.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace chdScoring.BusinessLogic.Extensions
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public ApiLoggingMiddleware(RequestDelegate next) => this._next = next;


        public async Task InvokeAsync(HttpContext context, IApiLogger logger)
        {
            //if(context.Request.)

            await logger.Log($"{context.Request.Method} {context.Request.Path}: {context.Request.Headers} {context.Request.Body}");
            await this._next(context);
        }
    }
}
