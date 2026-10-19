using System;
using System.Collections.Generic;
using System.Text;
using chd.Api.Base.Client;
using chd.Api.Base.Client.Extensions;
using chd.Api.Base.Contracts.Interfaces;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.Client.Clients
{
    public class StammdatenClient : BaseAuthenticationApiService, IStammdatenService
    {
        public StammdatenClient(ILogger<StammdatenClient> logger, IApiKeyProvider apiKeyProvider, IHttpClientFactory httpClientFactory) : base(logger, apiKeyProvider, httpClientFactory)
        {
        }

        public Task<ImageDto> GetImage(int id, CancellationToken cancellationToken)
            => this.Get<ImageDto>(Stammdaten.GET_Image.SetUrlParameters((nameof(id), id)), cancellationToken);

        public Task<StammdatenDto> Get(CancellationToken cancellationToken)
            => this.Get<StammdatenDto>(Stammdaten.GET_Stammdaten, cancellationToken);
    }
}
