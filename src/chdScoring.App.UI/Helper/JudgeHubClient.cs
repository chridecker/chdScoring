using chd.Api.Base.Contracts.Constants;
using chd.Api.Base.Contracts.Interfaces;
using chd.Hub.Base.Client;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace chdScoring.App.UI.Helper
{
    public class JudgeHubClient : BaseAuthenticationHubClient<IFlightHub>, IJudgeHubClient
    {
        private readonly IApiKeyProvider _apiKeyProvider;
        private readonly IJudgeDataCache _judgeDataCache;
        private readonly ISettingManager _settingManager;
        private readonly INotificationManagerService _notificationManagerService;

        public JudgeHubClient(ILogger<JudgeHubClient> logger, IApiKeyProvider apiKeyProvider, IJudgeDataCache judgeDataCache, ISettingManager settingManager, INotificationManagerService notificationManagerService) : base(logger, apiKeyProvider)
        {
            _apiKeyProvider = apiKeyProvider;
            this._judgeDataCache = judgeDataCache;
            this._settingManager = settingManager;
            this._notificationManagerService = notificationManagerService;
        }


        protected override Action<HttpConnectionOptions> ConfigureHttpOptions => AddApiKey2;


        protected void AddApiKey2(HttpConnectionOptions options)
        {
            if (!options.Headers.ContainsKey(ApiKeyConstants.HEADER_KEY))
            {
                var key = _apiKeyProvider.GetApiKey();
                options.Headers.Add(ApiKeyConstants.HEADER_KEY, key);
            }
        }

        public event EventHandler<CurrentFlight> DataReceived;

        protected override Uri LoadUri()
        {
            var baseAddress = this._settingManager.MainUrl.Result;
            return new UriBuilder($"{baseAddress}chdscoring/flight-hub").Uri;
        }

        protected override async Task<bool> ShouldInitialize(CancellationToken cancellationToken)
            => !string.IsNullOrWhiteSpace((await this._settingManager.MainUrl));

        protected override async Task DoInvokations(HubConnection connection, CancellationToken cancellationToken)
        {

        }

        protected override void SpecificReinitialize(HubConnection connection)
        {
            connection?.Remove(nameof(IFlightHub.ReceiveFlightData));
            connection?.Remove(nameof(IFlightHub.ReceiveNotification));
        }

        protected override void HookIncomingCalls(HubConnection connection)
        {
            connection.On<CurrentFlight>(nameof(IFlightHub.ReceiveFlightData), (dto) =>
            {
                this._judgeDataCache.Update(dto);
                this.DataReceived?.Invoke(this, dto);
            });

            connection.On<NotificationDto>(nameof(IFlightHub.ReceiveNotification), (dto) =>
            {
                this._notificationManagerService.SendNotification(dto.Title, dto.Message);
            });
        }

        public Task Register(int judge, CancellationToken cancellationToken = default)
        => base.SendAsync(async (conn) =>
             {
                 await conn.SendAsync(nameof(IFlightHub.RegisterAsJudge), judge, cancellationToken);
             });

        public Task RegisterControlCenter(CancellationToken cancellationToken = default)
        => this.SendAsync(async (conn) =>
            {
                await conn.SendAsync(nameof(IFlightHub.RegisterAsControlCenter), cancellationToken);
            });
    }

}
