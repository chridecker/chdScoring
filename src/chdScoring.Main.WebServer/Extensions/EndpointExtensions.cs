using chdScoring.BusinessLogic.Hubs;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
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

            var control = mainGroup.MapGroup(EndpointConstants.Control.ROUTE).WithTags(EndpointConstants.Control.ROUTE).RequireApiKeyAuth();

            var scoring = mainGroup.MapGroup(Scoring.ROUTE).WithTags(Scoring.ROUTE).RequireApiKeyAuth();
            var judges = mainGroup.MapGroup(Judge.ROUTE).WithTags(Judge.ROUTE).RequireApiKeyAuth();

            var pilot = mainGroup.MapGroup(Pilot.ROUTE).WithTags(Pilot.ROUTE).RequireApiKeyAuth();
            var database = mainGroup.MapGroup(Database.ROUTE).WithTags(Database.ROUTE).RequireApiKeyAuth();

            var print = mainGroup.MapGroup(Print.ROUTE).WithTags(Print.ROUTE);
            var import = mainGroup.MapGroup(Import.ROUTE).WithTags(Import.ROUTE);

            import.MapPost(Import.POST_BIN, async (ImportFileDto dto, IImportService service, CancellationToken ct) => await service.ImportBinFile(dto, ct));
            import.MapPost(Import.POST_JSON, async (ImportFileDto dto, IImportService service, CancellationToken ct) => await service.ImportJsonFile(dto, ct));
            import.MapPost(Import.POST_JSONRESULT, async (ImportFileDto dto, IImportService service, CancellationToken ct) => await service.ImportJsonResultFile(dto, ct));

            print.MapGet(Print.GET_PDF, async (IPrintService svc, CancellationToken ct) => await svc.GetPdfLst(ct));
            print.MapGet(Print.GET_AUTOPRINT, async (IPrintService svc, CancellationToken ct) => await svc.GetAutoPrintSetting(ct));
            print.MapPost(Print.POST_CHANGE_AUTOPRINT, async (IPrintService service, CancellationToken ct) => await service.ChangeAutoPrint(ct));
            print.MapPost(Print.POST_ADD, async (CreatePdfDto dto, IPrintService service, CancellationToken ct) => await service.PrintToPdfAsync(dto, ct));
            print.MapPost(Print.POST_PRINT_PDF, async (PrintPdfDto dto, IPrintService service, CancellationToken ct) => await service.AddToPrintCache(dto, ct));
            print.MapPost(Print.POST_DELETE, async (PrintPdfDto dto, IPrintService service, CancellationToken ct) => await service.DeleteFileAsync(dto, ct));


            database.MapGet(Database.GET, async (IDatabaseService service, CancellationToken token) => await service.GetDatabaseConnections(token));
            database.MapGet(Database.GET_CURRENT, async (IDatabaseService service, CancellationToken token) => await service.GetCurrentDatabaseConnection(token));
            database.MapPost(Database.POST_SETDATABASE, async (SetDatabaseConnectionDto dto, IDatabaseService service, CancellationToken token) => await service.SetDatabaseConnection(dto.ConnectionName, token));

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

            pilot.MapPost(EndpointConstants.Pilot.POST_Delete, async (PilotDto dto, IPilotService service, CancellationToken cancellationToken)
                => await service.Delete(dto, cancellationToken));



            control.MapPost(EndpointConstants.Control.POST_TIMER, async (TimerOperationDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.HandleOperation(dto, cancellationToken));

            control.MapPost(EndpointConstants.Control.POST_SaveRound, async (SaveRoundDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.SaveRound(dto, cancellationToken));

            control.MapPost(EndpointConstants.Control.POST_CalcRound, async (CalcRoundDto dto, ITimerService service, CancellationToken cancellationToken)
                => await service.CalculateRoundTBL(dto, cancellationToken));

            control.MapGet(EndpointConstants.Control.GET_OpenRound, async (ITimerService service, CancellationToken cancellationToken)
                => await service.GetFinishedRound(cancellationToken));


            judges.MapGet(Judge.GET_Flight, async (IJudgeService judgesService, CancellationToken cancellationToken) 
                => await judgesService.GetCurrentFlight(cancellationToken));

            judges.MapGet(Judge.GET_All, async (IJudgeService judgeService, CancellationToken cancellationToken)
                => await judgeService.GetJudges(cancellationToken));


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
