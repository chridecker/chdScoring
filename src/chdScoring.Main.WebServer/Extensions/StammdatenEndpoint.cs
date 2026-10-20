using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class StammdatenEndpoint
    {
        public static IEndpointRouteBuilder MapStammdaten(this IEndpointRouteBuilder app)
        {
            var stammdaten = app.MapGroup(Stammdaten.ROUTE).WithTags(Stammdaten.ROUTE);
            stammdaten.MapGet(Stammdaten.GET_Image,
                async (int id, IStammdatenService svc, CancellationToken ct) => await svc.GetImage(id, ct));

            stammdaten.MapGet(Stammdaten.GET_Stammdaten,
                async (IStammdatenService svc, CancellationToken ct) => await svc.Get(ct));

            stammdaten.MapGet(Stammdaten.GET_RoundResultPdfUrl,
                async (IStammdatenService svc, CancellationToken ct) => await svc.GetRoundResultPdfUrl(ct));
            return app;
        }
    }
}
