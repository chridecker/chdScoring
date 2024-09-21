using global::Microsoft.AspNetCore.Components;
using chdScoring.App.Helper;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Services;

namespace chdScoring.App.Pages
{
    public partial class Control : IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private CurrentFlight _dto;
        [Inject] IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] IJudgeDataCache _judgeDataCache { get; set; }
        [Inject] IMainService _mainService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this._cts = new();
            this._dto = this._judgeDataCache.Data;
            if (!this._judgeHubClient.IsConnected)
            {
                await this._judgeHubClient.StartAsync(this._cts.Token);

            }
            await this._judgeHubClient.RegisterControlCenter(this._cts.Token);

            this._judgeHubClient.DataReceived += this._judgeHubClient_DataReceived;

            await base.OnInitializedAsync();
        }
        private async void _judgeHubClient_DataReceived(object sender, CurrentFlight e)
        {
            this._dto = e;
            await this.InvokeAsync(this.StateHasChanged);
        }

        private Task StartStop()
        => _mainService.StartStop(this._dto.LeftTime.HasValue ? Contracts.Enums.ETimerOperation.Stop : Contracts.Enums.ETimerOperation.Start, this._cts.Token);


        public void Dispose()
        {
            this._judgeHubClient.DataReceived -= this._judgeHubClient_DataReceived;
            this._cts.Cancel();
        }
    }
}