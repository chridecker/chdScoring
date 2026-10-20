using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class ControlEndpoint
    {
        public static IEndpointRouteBuilder MapControl(this IEndpointRouteBuilder app)
        {
            var control = app.MapGroup(EndpointConstants.Control.ROUTE).WithTags(EndpointConstants.Control.ROUTE);
            control.MapPost(EndpointConstants.Control.POST_TIMER, async (TimerOperationDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.HandleOperation(dto, cancellationToken));

            control.MapPost(EndpointConstants.Control.POST_SaveRound, async (SaveRoundDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.SaveRound(dto, cancellationToken));

            control.MapPost(EndpointConstants.Control.POST_CalcRound, async (CalcRoundDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.CalculateRoundTBL(dto, cancellationToken));

            control.MapGet(EndpointConstants.Control.GET_OpenRound, async (ITimerService service, CancellationToken cancellationToken)
                => await service.GetFinishedRound(cancellationToken));

            control.MapPost(EndpointConstants.Control.POST_Start, async (ICompetitionService service, CancellationToken cancellationToken)
                => await service.StartAsync(cancellationToken));

            control.MapDelete(EndpointConstants.Control.DELETE_Clear, async (ICompetitionService service, CancellationToken cancellationToken)
                => await service.ClearDataAsync(cancellationToken));
            return app;
        }
    }
}
