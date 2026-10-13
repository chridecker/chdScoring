using chd.Api.Base.Client;
using chd.Api.Base.Client.Extensions;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using chd.Api.Base.Contracts.Interfaces;
using static chdScoring.Contracts.Constants.EndpointConstants.ApiKey;

namespace chdScoring.Main.Client.Clients
{
    public class ApiKeyClient(ILogger<ApiKeyClient> logger, IApiKeyProvider apiKeyProvider, IHttpClientFactory httpClientFactory)
        : BaseAuthenticationApiService(logger, apiKeyProvider, httpClientFactory), IApiKeyService
    {
        public Task<ApiKeyDto> GetAsync(string key, CancellationToken cancellationToken)
        => this.Get<ApiKeyDto>(GET.SetUrlParameters((nameof(key), key)), cancellationToken);

        public Task<List<ApiKeyDto>> GetAllAsync(CancellationToken cancellationToken)
        => this.Get<List<ApiKeyDto>>(GETALL, cancellationToken);

        public Task SaveAsync(ApiKeyDto dto, CancellationToken cancellationToken)
            => this.Post(SAVE, dto, cancellationToken);

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
            => this.Delete(DELETE.SetUrlParameters((nameof(id), id)), cancellationToken);
    }
}
