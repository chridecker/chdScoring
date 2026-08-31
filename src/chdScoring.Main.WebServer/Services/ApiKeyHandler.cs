using System;
using System.Collections.Generic;
using System.Text;
using chd.Api.Base.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;

namespace chdScoring.Main.WebServer.Services
{
    public class ApiKeyHandler(IConfiguration configuration) : IApiKeyHandler
    {
        public Task<bool> IsValid(string requestKey, CancellationToken cancellationToken = new CancellationToken())
        {
            var key = configuration.GetSection("X-Api-Key").Value;
            return Task.FromResult(string.Equals(key, requestKey));
        }

        public Task<string> GetCustomData(string requestKey, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(string.Empty);
        }
    }
}
