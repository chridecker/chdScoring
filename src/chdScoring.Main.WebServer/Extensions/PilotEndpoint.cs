using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class PilotEndpoint
    {
        public static IEndpointRouteBuilder MapPilot(this IEndpointRouteBuilder app)
        {
            var pilot = app.MapGroup(Pilot.ROUTE).WithTags(Pilot.ROUTE);
            pilot.MapGet(EndpointConstants.Pilot.GET_Img, async (int id, IPilotService service, CancellationToken cancellationToken)
              => await service.GetCountryImage(id, cancellationToken));

            pilot.MapGet(EndpointConstants.Pilot.GET_AllCountries, async (IPilotService service, CancellationToken cancellationToken)
                => await service.GetCountries(cancellationToken));

            pilot.MapGet(EndpointConstants.Pilot.GET_OpenRound, async (int? round, IPilotService service, CancellationToken cancellationToken)
                => await service.GetOpenRound(round, cancellationToken));

            pilot.MapGet(EndpointConstants.Pilot.GET_FinishedRounds, async (IPilotService service, CancellationToken cancellationToken)
                => await service.GetFinishedFlights(cancellationToken));

            pilot.MapGet(EndpointConstants.Pilot.GET_RoundResult, async (int? round, IPilotService service, CancellationToken cancellationToken)
                => await service.GetRoundResult(round, cancellationToken));

            pilot.MapGet(EndpointConstants.Pilot.GET_Round, async (int pilot, int round, IPilotService service, CancellationToken cancellationToken)
                => await service.GetRoundData(pilot, round, cancellationToken));

            pilot.MapGet(EndpointConstants.Pilot.GET_All, async (IPilotService service, CancellationToken cancellationToken)
                => await service.GetAllPilots(cancellationToken));

            pilot.MapGet(EndpointConstants.Pilot.GET_Startorder, async (int? round, IPilotService service, CancellationToken cancellationToken)
                => await service.GetStartingOrderPerRound(round, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_ChangeStartOrder, async ([FromQuery] int newStart, [FromBody] PilotRoundDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.ChangeStartNumber(newStart, dto, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_SetPilotActive, async (LoadPilotDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.SetPilotActive(dto, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_UnloadPilot, async (LoadPilotDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.UnLoadPilot(dto, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_SetStart, async (SetStartNumberDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.SetStartnumber(dto, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_Reflight, async (ReflightRoundDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.ReflightRound(dto, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_UPDATE_DATA, async (PilotDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.UpdatePilotData(dto, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_AddNew, async (PilotDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.Add(dto, cancellationToken));

            pilot.MapDelete(EndpointConstants.Pilot.DELETE_Delete, async ([FromBody] PilotDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.Delete(dto, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_Random, async (IPilotService service, CancellationToken cancellationToken)
                => await service.RandomizeAsync(cancellationToken));
            return app;
        }
    }
}
