using System;
using System.Collections.Generic;
using System.Text;
using chd.Api.Base.Client;
using chd.Api.Base.Contracts.Interfaces;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace chdScoring.Main.Client.Clients
{
    public class ImportClient : BaseAuthenticationApiService, IImportService
    {
        public ImportClient(ILogger<ImportClient> logger, IApiKeyProvider apiKeyProvider, IHttpClientFactory httpClientFactory) : base(logger, apiKeyProvider, httpClientFactory)
        {
        }

        public Task<bool> ImportJsonResultFile(ImportFileDto dto, CancellationToken cancellationToken)
            => this.Post<bool>(EndpointConstants.Import.POST_JSONRESULT, dto, cancellationToken);
    }
}
