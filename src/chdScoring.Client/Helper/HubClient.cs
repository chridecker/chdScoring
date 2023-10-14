using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace chdScoring.Client.Helper
{
    public class HubClient
    {
        public HubClient()
        {

        }
        private HubConnection? _hubConnection;
        private List<string> messages = new List<string>();
        private string? userInput;
        private string? messageInput;

        public event EventHandler<CurrentFlight> DataReceived;

        public async Task Initialize(NavigationManager navigation, CancellationToken cancellationToken)
        {
            this._hubConnection = new HubConnectionBuilder()
                .WithUrl(navigation.ToAbsoluteUri("/chdScoring/flight-hub"))
                .Build();

            _hubConnection.On<CurrentFlight>(nameof(IFlightHub.ReceiveFlightData), (dto) =>
            {
                this.DataReceived?.Invoke(this, dto);
            });

            await _hubConnection.StartAsync();
        }

        public async Task Register(int judge, CancellationToken cancellationToken)
        {
            if (_hubConnection is not null)
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
}
