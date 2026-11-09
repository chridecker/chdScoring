using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using chd.Api.Base.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace chdScoring.Main.WebServer.Services
{
    public class ApiKeyHandler(IOptionsMonitor<AppSettings> optionsMonitor, IBaseEntityRepository<ApiKey> apiKeyRepository) : IApiKeyHandler
    {
        public async Task<bool> IsValid(string requestKey, CancellationToken cancellationToken = default)
        {
            var key = optionsMonitor.CurrentValue.ApiKey;
            return await apiKeyRepository.Exists(x => x.Key == requestKey, cancellationToken) || string.Equals(key, requestKey);
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
