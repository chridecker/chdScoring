using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class PrintEndpoint
    {
        public static IEndpointRouteBuilder MapPrint(this IEndpointRouteBuilder app)
        {
            var print = app.MapGroup(Print.ROUTE).WithTags(Print.ROUTE);
            print.MapGet(Print.GET_PDF, async (IPrintService svc, CancellationToken ct) => await svc.GetPdfLst(ct));
            print.MapGet(Print.GET_AUTOPRINT, async (IPrintService svc, CancellationToken ct) => await svc.GetAutoPrintSetting(ct));
            print.MapPost(Print.POST_CHANGE_AUTOPRINT, async (IPrintService service, CancellationToken ct) => await service.ChangeAutoPrint(ct));
            print.MapPost(Print.POST_ADD, async (CreatePdfDto dto, IPrintService service, CancellationToken ct) => await service.PrintToPdfAsync(dto, ct));
            print.MapPost(Print.POST_PRINT_PDF, async (PrintPdfDto dto, IPrintService service, CancellationToken ct) => await service.AddToPrintCache(dto, ct));
            print.MapPost(Print.POST_DELETE, async (PrintPdfDto dto, IPrintService service, CancellationToken ct) => await service.DeleteFileAsync(dto, ct));
            return app;
        }
    }
}
