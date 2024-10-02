using global::Microsoft.AspNetCore.Components;
using chdScoring.App.Helper;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Services;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.Pages
{
    public partial class Control : ComponentBase, IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private CurrentFlight _dto;
        [Inject] IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] IJudgeDataCache _judgeDataCache { get; set; }
        [Inject] ITimerService _timerService { get; set; }

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
        private string _leftTime => (this._dto?.LeftTime.HasValue ?? false) && this._dto.LeftTime.Value <= this._dto.Round.Time ? 
            this._dto.LeftTime.Value > TimeSpan.Zero ? this._dto.LeftTime.Value.ToString("mm\\:ss") : TimeSpan.Zero.ToString("mm\\:ss") : this._dto.Round.Time.ToString("mm\\:ss");
        private string _faStartStop => (this._dto?.LeftTime.HasValue ?? false) ? "circle-stop" : "circle-play";

        private async void _judgeHubClient_DataReceived(object sender, CurrentFlight e)
        {
            this._dto = e;
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task StartStop()
        {
            try
            {
                var dto = new TimerOperationDto
                {
                    Airfield = 1,
                    Operation = this._dto.LeftTime.HasValue ? Contracts.Enums.ETimerOperation.Stop : Contracts.Enums.ETimerOperation.Start
                };
                await this._timerService.HandleOperation(dto, this._cts.Token);
            }
            catch
            {
            }
        }


        public void Dispose()
        {
            this._judgeHubClient.DataReceived -= this._judgeHubClient_DataReceived;
            this._cts.Cancel();
        }
    }
}