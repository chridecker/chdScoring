using chd.Api.Base.Client;
using chd.Api.Base.Client.Extensions;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using static chdScoring.Contracts.Constants.EndpointConstants.ApiKey;

namespace chdScoring.Main.Client.Clients
{
    public class ApiKeyClient(ILogger<ApiKeyClient> logger, IHttpClientFactory httpClientFactory)
        : BaseApiService(logger, httpClientFactory), IApiKeyService
    {
        public Task<List<ApiKeyDto>> GetAllAsync(CancellationToken cancellationToken)
        => this.Get<List<ApiKeyDto>>(GET, cancellationToken);

        public Task SaveAsync(ApiKeyDto dto, CancellationToken cancellationToken)
            => this.Post(SAVE, dto, cancellationToken);

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
            => this.Delete(DELETE.SetUrlParameters(("id", id)), cancellationToken);
    }
}
