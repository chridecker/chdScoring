using chdScoring.PrintService.Constants;
using chdScoring.PrintService.Dtos;
using chdScoring.PrintService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace chdScoring.PrintService.Extensions
{
    public static class EndpointExtensions
    {
        public static IEndpointRouteBuilder MapPrinterService(this IEndpointRouteBuilder app, string endpoint)
        {
            var mainGroup = app.MapGroup(endpoint).WithName(endpoint).WithDisplayName(endpoint).WithOpenApi();

            mainGroup.MapGet(EndpointConstants.ROUTE_Test, () => Results.Ok()).WithName(EndpointConstants.ROUTE_Test);

            mainGroup.MapPost(EndpointConstants.ROUTE_ADD_PRINT, (CreatePdfDto dto, IPrintCache cache) =>
            {
                cache.Add(dto);
                return Results.Ok();
            }).WithName(EndpointConstants.ROUTE_ADD_PRINT);

            return app;
        }
    }
}
