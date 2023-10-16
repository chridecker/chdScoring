using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Helper
{
    public abstract class HubClient<THub> : IHubClient<THub>, IAsyncDisposable
    {
        private HubConnection? _hubConnection;

        public async Task Initialize(CancellationToken cancellationToken = default)
        {
            if (this._hubConnection == null || this._hubConnection.State != HubConnectionState.Connected)
            {
                try
                {

                    var uri = await this.BaseAddress;

                    this._hubConnection = new HubConnectionBuilder()
                        .WithAutomaticReconnect()
                        .WithUrl(uri)
                        .Build();


                    this.OnInvokeHub(this._hubConnection);
                    await _hubConnection.StartAsync();
                }
                catch { }
            }
        }

        public async Task SendAsync(Func<HubConnection, Task> method, CancellationToken cancellationToken)
        {
            if (_hubConnection is not null && this._hubConnection.State == HubConnectionState.Connected)
            {
                await method(this._hubConnection);
            }
        }
        public bool IsConnected =>
           _hubConnection?.State == HubConnectionState.Connected;

        protected abstract Task<Uri> BaseAddress { get; }

        protected abstract void OnInvokeHub(HubConnection hubConnection);

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
    public interface IHubClient<THub> : IAsyncDisposable
    {
        Task Initialize(CancellationToken cancellationToken = default);
    }
}
