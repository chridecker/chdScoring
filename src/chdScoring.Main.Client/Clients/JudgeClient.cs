using chd.Api.Base.Client;
using chd.Api.Base.Contracts.Interfaces;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace chdScoring.Main.Client.Clients
{
    public class JudgeClient : BaseAuthenticationApiService, IJudgeService
    {
        public JudgeClient(ILogger<JudgeClient> logger, IApiKeyProvider apiKeyProvider, IHttpClientFactory httpClientFactory) : base(logger, apiKeyProvider, httpClientFactory)
        {
        }

        public Task<CurrentFlight> GetCurrentFlight(CancellationToken cancellationToken = default)
        => this.Get<CurrentFlight>(EndpointConstants.Judge.GET_Flight, cancellationToken);

        public Task<IEnumerable<JudgeDto>> GetJudges(CancellationToken cancellationToken = default)
            => this.Get<IEnumerable<JudgeDto>>(EndpointConstants.Judge.GET_All, cancellationToken);
    }
}
