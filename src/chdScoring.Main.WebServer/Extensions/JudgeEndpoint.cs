using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class JudgeEndpoint
    {
        public static IEndpointRouteBuilder MapJudge(this IEndpointRouteBuilder app)
        {
            var judges = app.MapGroup(Judge.ROUTE).WithTags(Judge.ROUTE);
            judges.MapGet(Judge.GET_Flight, async (IJudgeService judgesService, CancellationToken cancellationToken)
                => await judgesService.GetCurrentFlight(cancellationToken));

            judges.MapGet(Judge.GET_All, async (IJudgeService judgeService, CancellationToken cancellationToken)
                => await judgeService.GetJudges(cancellationToken));

            return app;
        }
    }
}
