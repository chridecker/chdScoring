using chdScoring.BusinessLogic.Hubs;
using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.UI.Extensions
{
    public static class EndpointExtensions
    {

        public static IEndpointRouteBuilder MapChdScoring(this IEndpointRouteBuilder app)
        {
            var mainGroup = app.MapGroup(ROOT).WithTags(ROOT);

            var control = mainGroup.MapGroup(EndpointConstants.Control.ROUTE).WithTags(EndpointConstants.Control.ROUTE);

            var scoring = mainGroup.MapGroup(Scoring.ROUTE).WithTags(Scoring.ROUTE);
            var judges = mainGroup.MapGroup(Judge.ROUTE).WithDisplayName(Judge.ROUTE);
            var pilot = mainGroup.MapGroup(Pilot.ROUTE).WithDisplayName(Pilot.ROUTE);

            pilot.MapGet(EndpointConstants.Pilot.GET_OpenRound, async (int? round, IPilotService service, CancellationToken cancellationToken)
                => await service.GetOpenRound(round, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_SetPilotActive, async (LoadPilotDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.SetPilotActive(dto, cancellationToken));

            control.MapPost(EndpointConstants.Control.POST_TIMER, async (TimerOperationDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.HandleOperation(dto, cancellationToken));

            control.MapPost(EndpointConstants.Control.POST_SaveRound, async (SaveRoundDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.SaveRound(dto, cancellationToken));


            judges.MapGet(Judge.GET_Flight, async (IJudgeService judgesService) => await judgesService.GetCurrentFlight());

            judges.MapGet(string.Empty, async (IJudgeService judgeService, CancellationToken cancellationToken)
                => await judgeService.GetJudges());


            scoring.MapPost(Scoring.POST_Save, async (SaveScoreDto dto, IScoringService service, IFlightCacheService cache, IHubContext<FlightHub, IFlightHub> hub, CancellationToken cancellationToken) =>
            {
                if (await service.SaveScore(dto, cancellationToken))
                {
                    await hub.Clients.Group($"judge{dto.Judge}").ReceiveFlightData(cache.GetCurrentFlight(DateTime.Now));
                    return true;
                }
                return false;
            });

            scoring.MapPost(Scoring.POST_Update, async (SaveScoreDto dto, IScoringService service, IFlightCacheService cache, IHubContext<FlightHub, IFlightHub> hub, CancellationToken cancellationToken) =>
            {
                if (await service.UpdateScore(dto, cancellationToken))
                {
                    await hub.Clients.Group($"judge{dto.Judge}").ReceiveFlightData(cache.GetCurrentFlight(DateTime.Now));
                    return true;
                }
                return false;
            });
            return app;
        }
    }
}
