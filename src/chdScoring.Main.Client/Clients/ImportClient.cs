using System;
using System.Collections.Generic;
using System.Text;
using chd.Api.Base.Client;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace chdScoring.Main.Client.Clients
{
    public class ImportClient : BaseApiService, IImportService
    {
        public ImportClient(ILogger<ImportClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public Task<bool> ImportBinFile(ImportFileDto dto, CancellationToken cancellationToken)
            => this.Post<bool>(EndpointConstants.Import.POST_BIN, dto, cancellationToken);

        public Task<bool> ImportJsonFile(ImportFileDto dto, CancellationToken cancellationToken)
            => this.Post<bool>(EndpointConstants.Import.POST_JSON, dto, cancellationToken);

        public Task<bool> ImportJsonResultFile(ImportFileDto dto, CancellationToken cancellationToken)
            => this.Post<bool>(EndpointConstants.Import.POST_JSONRESULT, dto, cancellationToken);
    }
}
