using global::Microsoft.AspNetCore.Components;
using chdScoring.App.Helper;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Services;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;
using chdScoring.App.Constants;
using chd.UI.Base.Components.Base;
using chdScoring.App.Interfaces;

namespace chdScoring.App.Pages
{
    public partial class Control : PageComponentBase<int,int>, IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private CurrentFlight _dto;

        [Inject] IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] IJudgeDataCache _judgeDataCache { get; set; }
        [Inject] ITimerService _timerService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.ControlCenter;
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


        public void Dispose()
        {
            this._judgeHubClient.DataReceived -= this._judgeHubClient_DataReceived;
            this._cts.Cancel();
        }
    }
}