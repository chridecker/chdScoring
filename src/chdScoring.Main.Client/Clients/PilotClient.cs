using chd.Api.Base.Client;
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

        public Task<ImageDto> GetCountryImage(int id, CancellationToken cancellationToken)
           => base.Get<ImageDto>(EndpointConstants.Pilot.GET_Img.SetUrlParameters((nameof(id), id)), cancellationToken);

        public Task<IEnumerable<CountryDto>> GetCountries(CancellationToken cancellationToken)
           => base.Get<IEnumerable<CountryDto>>(EndpointConstants.Pilot.GET_AllCountries, cancellationToken);


        public Task<IEnumerable<RoundResultDto>> GetRoundResult(int? round, CancellationToken cancellationToken)
           => base.Get<IEnumerable<RoundResultDto>>(round.HasValue ? EndpointConstants.Pilot.GET_RoundResult.SetUrlParameters(("round", round)) : EndpointConstants.Pilot.GET_RoundResult, cancellationToken);

        public Task<bool> SetPilotActive(LoadPilotDto dto, CancellationToken cancellationToken) => base.Post<bool>(EndpointConstants.Pilot.POST_SetPilotActive, dto, cancellationToken);
        public Task<bool> UnLoadPilot(LoadPilotDto dto, CancellationToken cancellationToken) => base.Post<bool>(EndpointConstants.Pilot.POST_UnloadPilot, dto, cancellationToken);

        public Task<IEnumerable<PilotDto>> GetAllPilots(CancellationToken cancellationToken = default)
        => base.Get<IEnumerable<PilotDto>>(EndpointConstants.Pilot.GET_All, cancellationToken);

        public Task<bool> SetStartnumber(SetStartNumberDto dto, CancellationToken cancellationToken = default)
        => this.Post<bool>(EndpointConstants.Pilot.POST_SetStart, dto, cancellationToken);

        public Task<bool> ReflightRound(ReflightRoundDto dto, CancellationToken cancellationToken = default)
        => this.Post<bool>(EndpointConstants.Pilot.POST_Reflight, dto, cancellationToken);
        public Task<bool> UpdatePilotData(PilotDto dto, CancellationToken cancellationToken = default)
        => this.Post<bool>(EndpointConstants.Pilot.POST_UPDATE_DATA, dto, cancellationToken);
        public Task<bool> Add(PilotDto dto, CancellationToken cancellationToken = default)
        => this.Post<bool>(EndpointConstants.Pilot.POST_AddNew, dto, cancellationToken);
        public Task<bool> Delete(PilotDto dto, CancellationToken cancellationToken = default)
        => this.Post<bool>(EndpointConstants.Pilot.POST_Delete, dto, cancellationToken);
    }
}
