using System;
using System.Collections.Generic;
using System.Text;
using chd.Api.Base.Client;
using chd.Api.Base.Contracts.Interfaces;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.Client.Clients
{
    public class CompetitionClient(
        ILogger<CompetitionClient> logger,
        IApiKeyProvider apiKeyProvider,
        IHttpClientFactory httpClientFactory)
        : BaseAuthenticationApiService(logger, apiKeyProvider, httpClientFactory), ICompetitionService
    {
        public Task ClearDataAsync(CancellationToken cancellationToken)
            => Delete(Control.DELETE_Clear, cancellationToken);
        
        public Task StartAsync(CancellationToken cancellationToken)
            => Post(Control.POST_Start, cancellationToken);
    }
}
