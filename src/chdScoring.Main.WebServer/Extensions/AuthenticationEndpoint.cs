using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class AuthenticationEndpoint
    {
        public static IEndpointRouteBuilder MapAuthentication(this IEndpointRouteBuilder app)
        {
            var authentication = app.MapGroup(EndpointConstants.Authentication.ROUTE).WithTags(EndpointConstants.Authentication.ROUTE);
            authentication.MapPost(Authentication.USER,
                async (LoginDto<int> dto, IAuthenticationService service, CancellationToken ct) =>
                    await service.GetUserAsync(dto, ct));

            authentication.MapGet(Authentication.USER_KEY,
                async (IAuthenticationService service, HttpContext context, CancellationToken ct) => await service.GetUserFromApiKeyAsync(context.User, ct));

            return app;
        }
    }
}
