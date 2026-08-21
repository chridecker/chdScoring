using chdScoring.Contracts.Interfaces;

namespace chdScoring.Web.Services
{
    public class ApiKeyHandler(IConfiguration configuration) : IApiKeyHandler
    {
        public Task<string> ApiKey => Task.FromResult(configuration.GetSection("ApiKey").Value);
    }
}
