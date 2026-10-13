using chd.Api.Base.Client;
using chd.Api.Base.Contracts.Interfaces;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace chdScoring.Main.Client.Clients
{
    public class ScoringClient : BaseAuthenticationApiService, IScoringService
    {
        public ScoringClient(ILogger<ScoringClient> logger, IApiKeyProvider apiKeyProvider, IHttpClientFactory httpClientFactory) : base(logger, apiKeyProvider, httpClientFactory)
        {
        }

        public Task<bool> ConfirmScores(ConfirmScoresDto saveScoreDto, CancellationToken cancellationToken)
        => this.Post<bool>(EndpointConstants.Scoring.POST_Confirm, saveScoreDto, cancellationToken);
        public Task<bool> UnConfirmScores(ConfirmScoresDto saveScoreDto, CancellationToken cancellationToken)
        => this.Post<bool>(EndpointConstants.Scoring.POST_UnConfirm, saveScoreDto, cancellationToken);

        public Task<bool> SaveScore(SaveScoreDto saveScoreDto, CancellationToken cancellationToken)
            => this.Post<bool>(EndpointConstants.Scoring.POST_Save, saveScoreDto, cancellationToken);

        public Task<bool> UpdateScore(SaveScoreDto saveScoreDto, CancellationToken cancellationToken)
            => this.Post<bool>(EndpointConstants.Scoring.POST_Update, saveScoreDto, cancellationToken);
    }
}
