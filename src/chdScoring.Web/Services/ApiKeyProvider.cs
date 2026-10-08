using chd.Api.Base.Contracts.Interfaces;

namespace chdScoring.Web.Services
{
    public class ApiKeyProvider(IConfiguration configuration) : IApiKeyProvider
    {
        public string GetApiKey() => configuration.GetSection("X-Api-Key").Value;
    }
}
