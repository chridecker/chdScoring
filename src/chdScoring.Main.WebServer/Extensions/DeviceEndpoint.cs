using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.WebServer.Extensions
{
    public static class DeviceEndpoint
    {
        public static IEndpointRouteBuilder MapDevice(this IEndpointRouteBuilder app)
        {
            var device = app.MapGroup(Device.ROUTE).WithTags(Device.ROUTE);
            device.MapGet(Device.GETALL, async (IDeviceService svc, CancellationToken ct) => await svc.GetAllAsync(ct));
            device.MapPost(Device.Identify, async ([FromBody] DeviceStatusDto dto, IDeviceService svc, CancellationToken ct) => await svc.Identify(dto, ct));
            device.MapPost(Device.Navigate, async ([FromQuery] string redirect, [FromBody] DeviceStatusDto dto, IDeviceService svc, CancellationToken ct) => await svc.NavigateTo(dto, redirect, ct));
            device.MapPost(Device.Shutdown, async ([FromQuery] bool restart, [FromBody] DeviceStatusDto dto, IDeviceService svc, CancellationToken ct) => await svc.ShutDown(dto, restart, ct));
            device.MapPost(Device.SetApiKey, async ([FromQuery] string apiKey, [FromBody] DeviceStatusDto dto, IDeviceService svc, CancellationToken ct) => await svc.SetApiKey(dto, apiKey, ct));
            device.MapPost(Device.SetAutoRedirect, async ([FromQuery] string redirect, [FromBody] DeviceStatusDto dto, IDeviceService svc, CancellationToken ct) => await svc.SetAutoRedirect(dto, redirect, ct));
            device.MapPost(Device.SetZoom, async ([FromQuery] int zoom, [FromBody] DeviceStatusDto dto, IDeviceService svc, CancellationToken ct) => await svc.SetZoom(dto, zoom, ct));
            device.MapPost(Device.ToggleTheme, async (DeviceStatusDto dto, IDeviceService svc, CancellationToken ct) => await svc.ToggleTheme(dto, ct));

            return app;
        }
    }
}
