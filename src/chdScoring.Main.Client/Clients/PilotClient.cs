﻿using chd.Api.Base.Client;
using chd.Api.Base.Client.Extensions;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace chdScoring.Main.Client.Clients
{
    public class PilotClient : BaseApiService, IPilotService
    {
        public PilotClient(ILogger<PilotClient> logger, IHttpClientFactory httpClientFactory) : base(logger, httpClientFactory)
        {
        }

        public async Task<IEnumerable<FinishedRoundDto>> GetFinishedFlights(CancellationToken cancellationToken = default)
        => await this.Get<IEnumerable<FinishedRoundDto>>(EndpointConstants.Pilot.GET_FinishedRounds, cancellationToken);
        public Task<IEnumerable<OpenRoundDto>> GetOpenRound(int? round, CancellationToken cancellationToken)
             => base.Get<IEnumerable<OpenRoundDto>>(round.HasValue ? EndpointConstants.Pilot.GET_OpenRound.SetUrlParameters(("round", round)) : EndpointConstants.Pilot.GET_OpenRound, cancellationToken);

        public Task<RoundDataDto> GetRoundData(int pilot, int round, CancellationToken cancellationToken)
           => base.Get<RoundDataDto>(EndpointConstants.Pilot.GET_Round.SetUrlParameters((nameof(pilot), pilot), (nameof(round), round)), cancellationToken);


        public Task<IEnumerable<RoundResultDto>> GetRoundResult(int? round, CancellationToken cancellationToken)
           => base.Get<IEnumerable<RoundResultDto>>(round.HasValue ? EndpointConstants.Pilot.GET_RoundResult.SetUrlParameters(("round", round)) : EndpointConstants.Pilot.GET_RoundResult, cancellationToken);

        public Task<bool> SetPilotActive(LoadPilotDto dto, CancellationToken cancellationToken) => base.Post<bool>(EndpointConstants.Pilot.POST_SetPilotActive, dto, cancellationToken);
        public Task<bool> UnLoadPilot(LoadPilotDto dto, CancellationToken cancellationToken) => base.Post<bool>(EndpointConstants.Pilot.POST_UnloadPilot, dto, cancellationToken);
    }
}
