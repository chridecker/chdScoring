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

            control.MapPost(EndpointConstants.Control.POST_TIMER, async (TimerOperationDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.HandleOperation(dto, cancellationToken));


            judges.MapGet(Judge.GET_Flight, (IJudgeService judgesService) => Results.Ok(judgesService.GetCurrentFlight()));

            judges.MapGet(string.Empty, async (IJudgeService judgeService, CancellationToken cancellationToken)
                => await judgeService.GetJudges());


            scoring.MapPost(Scoring.POST_Save, async (SaveScoreDto dto, IScoringService service, IFlightCacheService cache, IHubContext<FlightHub, IFlightHub> hub, CancellationToken cancellationToken) =>
            {
                if (await service.SaveScore(dto, cancellationToken))
                {
                    await hub.Clients.Group($"judge{dto.Judge}").ReceiveFlightData(cache.GetCurrentFlight());
                    return true;
                }
                return false;
            });
            return app;
        }
    }
}
