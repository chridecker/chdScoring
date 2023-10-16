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
    public class JudgeHubClient : IJudgeHubClient
    {
        private HubConnection? _hubConnection;
        private readonly IJudgeDataCache _judgeDataCache;
        private readonly ISettingManager _settingManager;


        public JudgeHubClient(IJudgeDataCache judgeDataCache, ISettingManager settingManager)
        {
            this._judgeDataCache = judgeDataCache;
            this._settingManager = settingManager;
        }

        public event EventHandler<CurrentFlight> DataReceived;

        public async Task Initialize(CancellationToken cancellationToken = default)
        {
            if (this._hubConnection == null || this._hubConnection.State != HubConnectionState.Connected)
            {
                try
                {

                    var baseAddress = await this._settingManager.MainUrl;
                    var uri = new UriBuilder($"{baseAddress}flight-hub").Uri;

                    this._hubConnection = new HubConnectionBuilder()
                        .WithUrl(uri)
                        .Build();

                    _hubConnection.On<CurrentFlight>(nameof(IFlightHub.ReceiveFlightData), (dto) =>
                    {
                        this._judgeDataCache.Update(dto);
                        this.DataReceived?.Invoke(this, dto);
                    });
                    await _hubConnection.StartAsync();
                }
                catch { }
            }
        }

        public async Task Register(int judge, CancellationToken cancellationToken = default)
        {
            if (_hubConnection is not null && this._hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.SendAsync(nameof(IFlightHub.RegisterAsJudge), judge, cancellationToken);
            }
        }

        public bool IsConnected =>
            _hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
    public interface IJudgeHubClient
    {
        event EventHandler<CurrentFlight> DataReceived;

        Task Initialize(CancellationToken cancellationToken = default);
        Task Register(int judge, CancellationToken cancellationToken = default);
    }
}
