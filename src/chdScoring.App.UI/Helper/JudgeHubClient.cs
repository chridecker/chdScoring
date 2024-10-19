using chd.Hub.Base.Client;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace chdScoring.App.UI.Helper
{
    public class JudgeHubClient : BaseHubClient<IFlightHub>, IJudgeHubClient
    {
        private readonly IJudgeDataCache _judgeDataCache;
        private readonly ISettingManager _settingManager;
        private readonly INotificationManagerService _notificationManagerService;

        public JudgeHubClient(ILogger<JudgeHubClient> logger, IJudgeDataCache judgeDataCache, ISettingManager settingManager, INotificationManagerService notificationManagerService) : base(logger)
        {
            this._judgeDataCache = judgeDataCache;
            this._settingManager = settingManager;
            this._notificationManagerService = notificationManagerService;
        }

        public event EventHandler<CurrentFlight> DataReceived;

        protected override Uri LoadUri()
        {
            var baseAddress = this._settingManager.MainUrl.Result;
            return new UriBuilder($"{baseAddress}chdscoring/flight-hub").Uri;
        }

        protected override async Task<bool> ShouldInitialize(CancellationToken cancellationToken)
            => !string.IsNullOrWhiteSpace((await this._settingManager.MainUrl));

        protected override Task DoInvokations(HubConnection connection, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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

            connection.On(nameof(IFlightHub.ReceiveStatusRequest), async () =>
            {
                await this.SendAsync(async (conn) =>
                {
                    await conn.SendAsync(nameof(IFlightHub.SendStatus),new DeviceStatusDto());
                });
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
        public Task RegisterStatus(CancellationToken cancellationToken = default)
        => this.SendAsync(async (conn) =>
            {
                await conn.SendAsync(nameof(IFlightHub.RegisterAsStatus), cancellationToken);
            });
    }

}
