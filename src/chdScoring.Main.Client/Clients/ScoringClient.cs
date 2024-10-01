using chd.Api.Base.Client;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.Main.Client.Clients
{
    public class ScoringClient : BaseApiService, IScoringService
    {
        public ScoringClient(ILogger<ScoringClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public Task<bool> SaveScore(SaveScoreDto saveScoreDto, CancellationToken cancellationToken)
       => this.Post<bool>(EndpointConstants.Scoring.POST_Save, saveScoreDto, cancellationToken);
    }
}
