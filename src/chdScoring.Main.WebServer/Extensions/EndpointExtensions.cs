using chdScoring.BusinessLogic.Hubs;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class EndpointExtensions
    {

        public static IEndpointRouteBuilder MapChdScoring(this IEndpointRouteBuilder app)
        {
            var mainGroup = app.MapGroup(ROOT).WithTags(ROOT);

            var control = mainGroup.MapGroup(EndpointConstants.Control.ROUTE).WithTags(EndpointConstants.Control.ROUTE);

            var scoring = mainGroup.MapGroup(Scoring.ROUTE).WithTags(Scoring.ROUTE);
            var judges = mainGroup.MapGroup(Judge.ROUTE).WithTags(Judge.ROUTE);
            var pilot = mainGroup.MapGroup(Pilot.ROUTE).WithTags(Pilot.ROUTE);
            var device = mainGroup.MapGroup(Device.ROUTE).WithTags(Device.ROUTE);
            var database = mainGroup.MapGroup(Database.ROUTE).WithTags(Database.ROUTE);
            var print = mainGroup.MapGroup(Print.ROUTE).WithTags(Print.ROUTE);

            print.MapGet(Print.GET_PDF, async (IPrintService svc, CancellationToken ct) => await svc.GetPdfLst(ct));
            print.MapGet(Print.GET_AUTOPRINT, async (IPrintService svc, CancellationToken ct) => await svc.GetAutoPrintSetting(ct));
            print.MapPost(Print.POST_CHANGE_AUTOPRINT, async (IPrintService service, CancellationToken ct) => await service.ChangeAutoPrint(ct));
            print.MapPost(Print.POST_ADD, async (CreatePdfDto dto, IPrintService service, CancellationToken ct) => await service.PrintToPdfAsync(dto, ct));
            print.MapPost(Print.POST_PRINT_PDF, async (PrintPdfDto dto, IPrintService service, CancellationToken ct) => await service.AddToPrintCache(dto, ct));


            database.MapGet(Database.GET, async (IDatabaseService service, CancellationToken token) => await service.GetDatabaseConnections(token));
            database.MapGet(Database.GET_CURRENT, async (IDatabaseService service, CancellationToken token) => await service.GetCurrentDatabaseConnection(token));
            database.MapPost(Database.POST_SETDATABASE, async (SetDatabaseConnectionDto dto, IDatabaseService service, CancellationToken token) => await service.SetDatabaseConnection(dto.ConnectionName, token));

            device.MapGet(Device.GET, async ([FromServices] IDeviceService service, CancellationToken token) => await service.GetAll(token));
            device.MapGet(Device.GET_DeviceStatus, async ([FromQuery] string name, [FromServices] IDeviceService service, CancellationToken cancellation) => await service.GetByName(name, cancellation));

            pilot.MapGet(EndpointConstants.Pilot.GET_OpenRound, async (int? round, IPilotService service, CancellationToken cancellationToken)
                => await service.GetOpenRound(round, cancellationToken));

            pilot.MapGet(EndpointConstants.Pilot.GET_RoundResult, async (int? round, IPilotService service, CancellationToken cancellationToken)
                => await service.GetRoundResult(round, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_SetPilotActive, async (LoadPilotDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.SetPilotActive(dto, cancellationToken));

            pilot.MapPost(EndpointConstants.Pilot.POST_UnloadPilot, async (LoadPilotDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.UnLoadPilot(dto, cancellationToken));

            control.MapPost(EndpointConstants.Control.POST_TIMER, async (TimerOperationDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.HandleOperation(dto, cancellationToken));

            control.MapPost(EndpointConstants.Control.POST_SaveRound, async (SaveRoundDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.SaveRound(dto, cancellationToken));

            control.MapPost(EndpointConstants.Control.POST_CalcRound, async (CalcRoundDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.CalculateRoundTBL(dto, cancellationToken));

            control.MapGet(EndpointConstants.Control.GET_OpenRound, async (ITimerService service, CancellationToken cancellationToken)
                => await service.GetFinishedRound(cancellationToken));


            judges.MapGet(Judge.GET_Flight, async (IJudgeService judgesService) => await judgesService.GetCurrentFlight());

            judges.MapGet(Judge.GET_All, async (IJudgeService judgeService, CancellationToken cancellationToken)
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
