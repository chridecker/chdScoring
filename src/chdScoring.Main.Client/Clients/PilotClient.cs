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

        public Task<IEnumerable<OpenRoundDto>> GetOpenRound(int? round, CancellationToken cancellationToken)
             => base.Get<IEnumerable<OpenRoundDto>>(round.HasValue ? EndpointConstants.Pilot.GET_OpenRound.SetUrlParameters(("round", round)) : EndpointConstants.Pilot.GET_OpenRound, cancellationToken);
        public Task<IEnumerable<RoundResultDto>> GetRoundResult(int? round, CancellationToken cancellationToken)
           => base.Get<IEnumerable<RoundResultDto>>(round.HasValue ? EndpointConstants.Pilot.GET_RoundResult.SetUrlParameters(("round", round)) : EndpointConstants.Pilot.GET_RoundResult, cancellationToken);

        public Task<bool> SetPilotActive(LoadPilotDto dto, CancellationToken cancellationToken) => base.Post<bool>(EndpointConstants.Pilot.POST_SetPilotActive, dto, cancellationToken);
        public Task<bool> UnLoadPilot(LoadPilotDto dto, CancellationToken cancellationToken) => base.Post<bool>(EndpointConstants.Pilot.POST_UnloadPilot, dto, cancellationToken);
    }
}
