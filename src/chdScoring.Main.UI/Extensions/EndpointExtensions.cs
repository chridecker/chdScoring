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

namespace chdScoring.Main.UI.Extensions
{
    public static class EndpointExtensions
    {

        public static IEndpointRouteBuilder MapChdScoring(this IEndpointRouteBuilder app, string endpoint)
        {
            var mainGroup = app.MapGroup(endpoint).WithName(endpoint).WithDisplayName(endpoint).WithOpenApi();

            var control = mainGroup.MapGroup(EndpointConstants.ROUTE_Control).WithDisplayName("Control");

            var scoring = mainGroup.MapGroup(EndpointConstants.ROUTE_Scoring).WithDisplayName("Scoring");
            var judges = mainGroup.MapGroup(EndpointConstants.ROUTE_Judge).WithDisplayName("Judges");

            mainGroup.MapGet(EndpointConstants.GET_Test_Connection, () => Results.Ok())
                .WithName(EndpointConstants.GET_Test_Connection);

            control.MapPost(EndpointConstants.POST_TimerOperation, async (TimerOperationDto dto, IFlightCacheService cache, ITimerService service, IHubDataService hub, CancellationToken cancellationToken)
                =>
            {
                if (await service.HandleOperation(dto, cancellationToken))
                {
                    await cache.Update(cancellationToken);
                    await hub.SendAll(cancellationToken);
                    Results.Ok(true);
                }
                return Results.Ok(false);
            })
                .WithName(EndpointConstants.POST_TimerOperation);


            judges.MapGet($"{EndpointConstants.GET_Flight}", (IFlightCacheService flightCacheService) => Results.Ok(flightCacheService.GetCurrentFlight()))
                .WithName(EndpointConstants.GET_Flight);

            judges.MapGet(string.Empty, async (IJudgeRepository judgeRepo, CancellationToken cancellationToken) =>
            {
                var judges = await judgeRepo.FindAll(cancellationToken);
                return judges.Select(s => new JudgeDto { Id = s.Id, Name = $"{s.Vorname} {s.Name}" });
            }).WithName(string.Empty);


            scoring.MapPost(EndpointConstants.POST_Save, async (SaveScoreDto dto, IScoreService service, IFlightCacheService cache, IHubDataService hub, CancellationToken cancellationToken) =>
            {
                if (await service.SaveScore(dto, cancellationToken))
                {
                    cache.UpdateScore(dto);
                    await hub.SendJudge(dto.Judge, cancellationToken);
                    return Results.Ok();
                }
                return Results.BadRequest();
            }).WithName(EndpointConstants.POST_Save);




            return app;
        }
    }
}
