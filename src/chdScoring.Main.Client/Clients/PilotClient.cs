using chd.Api.Base.Client;
using chd.Api.Base.Client.Extensions;
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
    public class PilotClient : BaseApiService, IPilotService
    {
        public PilotClient(ILogger<PilotClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public Task<IEnumerable<OpenRoundDto>> GetOpenRound(int round, CancellationToken cancellationToken)
             => base.Get<IEnumerable<OpenRoundDto>>(EndpointConstants.Pilot.GET_OpenRound.SetUrlParameters(("round", 1)), cancellationToken);
    }
}
