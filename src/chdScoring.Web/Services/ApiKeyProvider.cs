using chd.Api.Base.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
using Microsoft.Extensions.Options;

namespace chdScoring.Web.Services
{
    public class ApiKeyProvider(IOptionsMonitor<AppSettings> optionsMonitor) : IApiKeyProvider
    {
        public string GetApiKey() => optionsMonitor.CurrentValue.ApiKey;
    }
}
