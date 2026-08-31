using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using chd.Api.Base.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Repositories;
using Microsoft.Extensions.Configuration;

namespace chdScoring.Main.WebServer.Services
{
    public class ApiKeyHandler(IConfiguration configuration, IApiKeyRepository apiKeyRepository) : IApiKeyHandler
    {
        public async Task<bool> IsValid(string requestKey, CancellationToken cancellationToken = default)
        {
            var key = configuration.GetSection("X-Api-Key").Value;
            return await apiKeyRepository.Exists(requestKey, cancellationToken) || string.Equals(key, requestKey);
        }

        public async Task<string> GetCustomData(string requestKey, CancellationToken cancellationToken = default)
        {
            var entry = await apiKeyRepository.FirstOrDefaultAsync(x => x.Key == requestKey, cancellationToken);
            if (entry is not null)
            {
                return JsonSerializer.Serialize(entry);
            }

            return string.Empty;
        }
    }
}
