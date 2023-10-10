using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
            var time = DateTime.Now;
            await logger.Log($"{time}: {context.Request.Host} {context.Request.Method} {context.Request.Path}");
            await this._next(context);
        }
    }
}
