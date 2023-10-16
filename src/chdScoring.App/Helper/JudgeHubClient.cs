using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace chdScoring.App.Helper
{
    public class JudgeHubClient : HubClient<IFlightHub>, IJudgeHubClient
    {
        private readonly IJudgeDataCache _judgeDataCache;
        private readonly ISettingManager _settingManager;


        public JudgeHubClient(IJudgeDataCache judgeDataCache, ISettingManager settingManager)
        {
            this._judgeDataCache = judgeDataCache;
            this._settingManager = settingManager;
        }

        public event EventHandler<CurrentFlight> DataReceived;

        protected override Task<Uri> BaseAddress
        => Task.Run(async () =>
        {
            var baseAddress = await this._settingManager.MainUrl;
            return new UriBuilder($"{baseAddress}flight-hub").Uri;
        });

        protected override void OnInvokeHub(HubConnection hubConnection)
        {
            hubConnection.On<CurrentFlight>(nameof(IFlightHub.ReceiveFlightData), (dto) =>
                   {
                       this._judgeDataCache.Update(dto);
                       this.DataReceived?.Invoke(this, dto);
                   });
        }

        public Task Register(int judge, CancellationToken cancellationToken = default)
        => base.SendAsync(async (conn) =>
             {
                 await conn.SendAsync(nameof(IFlightHub.RegisterAsJudge), judge, cancellationToken);
             }, cancellationToken);

        public Task RegisterControlCenter(CancellationToken cancellationToken = default)
        => this.SendAsync(async (conn) =>
            {
                await conn.SendAsync(nameof(IFlightHub.RegisterAsControlCenter), cancellationToken);
            }, cancellationToken);




    }
    public interface IJudgeHubClient : IHubClient<IFlightHub>
    {
        event EventHandler<CurrentFlight> DataReceived;
        Task Register(int judge, CancellationToken cancellationToken = default);
        Task RegisterControlCenter(CancellationToken cancellationToken = default);
    }
}
