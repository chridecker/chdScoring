using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.BusinessLogic.Hubs;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class ScoringEndpoint
    {
        public static IEndpointRouteBuilder MapScoring(this IEndpointRouteBuilder app)
        {
            var scoring = app.MapGroup(Scoring.ROUTE).WithTags(Scoring.ROUTE);

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
            scoring.MapPost(Scoring.POST_Confirm, async (ConfirmScoresDto dto, IScoringService service, IFlightCacheService cache, IHubContext<FlightHub, IFlightHub> hub, CancellationToken cancellationToken) =>
            {
                if (await service.ConfirmScores(dto, cancellationToken))
                {
                    await hub.Clients.Group($"judge{dto.Judge}").ReceiveFlightData(cache.GetCurrentFlight(DateTime.Now));
                    return true;
                }
                return false;
            });
            scoring.MapPost(Scoring.POST_UnConfirm, async (ConfirmScoresDto dto, IScoringService service, IFlightCacheService cache, IHubContext<FlightHub, IFlightHub> hub, CancellationToken cancellationToken) =>
            {
                if (await service.UnConfirmScores(dto, cancellationToken))
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
