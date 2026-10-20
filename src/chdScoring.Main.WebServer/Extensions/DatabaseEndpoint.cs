using chd.UI.Base.Contracts.Dtos.Authentication;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class DatabaseEndpoint
    {
        public static IEndpointRouteBuilder MapDatabase(this IEndpointRouteBuilder app)
        {
            var database = app.MapGroup(Database.ROUTE).WithTags(Database.ROUTE);
            database.MapGet(Database.GET, async (IDatabaseService service, CancellationToken token) => await service.GetDatabaseConnections(token));
            database.MapGet(Database.GET_CURRENT, async (IDatabaseService service, CancellationToken token) => await service.GetCurrentDatabaseConnection(token));
            database.MapPost(Database.POST_SETDATABASE, async (SetDatabaseConnectionDto dto, IDatabaseService service, CancellationToken token) => await service.SetDatabaseConnection(dto.ConnectionName, token));


            return app;
        }
    }
}
