using chd.Api.Base.Client;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.Client.Clients
{
    public class PrintClient : BaseApiService, IPrintService
    {
        public PrintClient(ILogger<BaseApiService> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public async Task<bool> PrintToPdfAsync(CreatePdfDto dto, CancellationToken cancellationToken = default)
        => await this.Post<bool>(Print.POST_ADD, dto, cancellationToken);
    }
}
