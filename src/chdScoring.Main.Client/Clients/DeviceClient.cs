using System;
using System.Collections.Generic;
using System.Text;
using chd.Api.Base.Client;
using chd.Api.Base.Client.Extensions;
using chd.Api.Base.Contracts.Interfaces;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.Main.Client.Clients
{
    public class DeviceClient : BaseAuthenticationApiService, IDeviceService
    {
        public DeviceClient(ILogger<DeviceClient> logger, IApiKeyProvider apiKeyProvider, IHttpClientFactory httpClientFactory) : base(logger, apiKeyProvider, httpClientFactory)
        {
        }

        public Task<List<DeviceStatusDto>> GetAllAsync(CancellationToken cancellationToken)
            => base.Get<List<DeviceStatusDto>>(Device.GETALL, cancellationToken);

        public Task NavigateTo(DeviceStatusDto dto, string redirect, CancellationToken cancellationToken)
            => this.Post(Device.Navigate.SetUrlParameters((nameof(redirect), redirect)), dto, cancellationToken);


        public Task SetAutoRedirect(DeviceStatusDto dto, string redirect, CancellationToken cancellationToken)
            => this.Post(Device.SetAutoRedirect.SetUrlParameters((nameof(redirect), redirect)), dto, cancellationToken);


        public Task SetApiKey(DeviceStatusDto dto, string apiKey, CancellationToken cancellationToken)
            => this.Post(Device.SetApiKey.SetUrlParameters((nameof(apiKey), apiKey)), dto, cancellationToken);


        public Task SetZoom(DeviceStatusDto dto, int zoom, CancellationToken cancellationToken)
            => this.Post(Device.SetZoom.SetUrlParameters((nameof(zoom), zoom)), dto, cancellationToken);


        public Task ShutDown(DeviceStatusDto dto, bool restart, CancellationToken cancellationToken)
            => this.Post(Device.Shutdown.SetUrlParameters((nameof(restart), restart)), dto, cancellationToken);

        public Task Identify(DeviceStatusDto dto, CancellationToken cancellationToken)
            => this.Post(Device.Identify, dto, cancellationToken);

        public Task ToggleTheme(DeviceStatusDto dto, CancellationToken cancellationToken)
            => this.Post(Device.ToggleTheme, dto, cancellationToken);
    }
}
