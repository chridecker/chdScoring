using chd.Api.Base.Client;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace chdScoring.Main.Client.Clients
{
    public class ScoringClient : BaseApiService, IScoringService
    {
        public ScoringClient(ILogger<ScoringClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public Task<bool> SaveScore(SaveScoreDto saveScoreDto, CancellationToken cancellationToken)
            => this.Post<bool>(EndpointConstants.Scoring.POST_Save, saveScoreDto, cancellationToken);

        public Task<bool> UpdateScore(SaveScoreDto saveScoreDto, CancellationToken cancellationToken)
            => this.Post<bool>(EndpointConstants.Scoring.POST_Update, saveScoreDto, cancellationToken);
    }
}
