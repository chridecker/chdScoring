using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class ImportEndpoint
    {
        public static IEndpointRouteBuilder MapImport(this IEndpointRouteBuilder app)
        {
            var import = app.MapGroup(Import.ROUTE).WithTags(Import.ROUTE);
            import.MapPost(Import.POST_JSONRESULT, async (ImportFileDto dto, IImportService service, CancellationToken ct) => await service.ImportJsonResultFile(dto, ct));

            return app;
        }
    }
}
