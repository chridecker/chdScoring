using chd.Api.Base.Contracts.Constants;
using chd.Api.Base.Extensions;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.BusinessLogic.Hubs;
using chdScoring.BusinessLogic.Services;
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
            var mainGroup = app.MapGroup(ROOT).WithTags(ROOT).RequireApiKeyAuth();

            mainGroup.MapApiKey();
            mainGroup.MapDevice();
            mainGroup.MapAuthentication();
            mainGroup.MapControl();
            mainGroup.MapPilot();
            mainGroup.MapScoring();
            mainGroup.MapJudge();
            mainGroup.MapStammdaten();
            mainGroup.MapImport();
            mainGroup.MapDatabase();
            mainGroup.MapPrint();

            return app;
        }
    }
}
