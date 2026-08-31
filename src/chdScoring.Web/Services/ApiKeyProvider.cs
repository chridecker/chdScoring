using chd.Api.Base.Contracts.Interfaces;

namespace chdScoring.Web.Services
{
    public class ApiKeyProvider(IConfiguration configuration) : IApiKeyProvider
    {
        public Task<string> GetApiKeyAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(configuration.GetSection("X-Api-Key").Value);
        }
    }
}
