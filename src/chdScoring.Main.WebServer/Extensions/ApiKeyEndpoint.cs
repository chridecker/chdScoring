using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class ApiKeyEndpoint
    {
        public static IEndpointRouteBuilder MapApiKey(this IEndpointRouteBuilder app)
        {
            var apiKey = app.MapGroup(EndpointConstants.ApiKey.ROUTE).WithTags(EndpointConstants.ApiKey.ROUTE);
            apiKey.MapGet(ApiKey.GET, async (string key, IApiKeyService service, CancellationToken ct) => await service.GetAsync(key, ct));
            apiKey.MapGet(ApiKey.GETALL, async (IApiKeyService service, CancellationToken ct) => await service.GetAllAsync(ct));
            apiKey.MapPost(ApiKey.SAVE, async (ApiKeyDto dto, IApiKeyService service, CancellationToken ct) => await service.SaveAsync(dto, ct));
            apiKey.MapDelete(ApiKey.DELETE, async (int id, IApiKeyService service, CancellationToken ct) => await service.DeleteAsync(id, ct));
            return app;
        }
    }
}
