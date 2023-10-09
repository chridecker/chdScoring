using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Dtos;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string body = string.Empty;
            if (context.Request.Method == HttpMethods.Post
                && context.Request.Path.Value.EndsWith("score/save"))
            {
                var score = await JsonSerializer.DeserializeAsync<SaveScoreDto>(context.Request.Body);
            }

            await logger.Log($"{context.Request.Method} {context.Request.Path}: " +
                $"{string.Join(",", context.Request.Query.Select(s => $"{s.Key}->{s.Value}"))} {body}");
            await this._next(context);
        }
    }
}
