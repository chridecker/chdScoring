using global::Microsoft.AspNetCore.Components;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.App.UI.Constants;
using chd.UI.Base.Components.Base;
using chdScoring.App.UI.Interfaces;

namespace chdScoring.App.UI.Pages
{
    public partial class Control : PageComponentBase<int, int>, IDisposable
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

        private decimal? _score(JudgeDto judge, ManeouvreDto maneouvre) => this._dto.ManeouvreLst[judge.Id].FirstOrDefault(x => x.Id == maneouvre.Id)?.Score;
        private string _scoreClass(JudgeDto judge, ManeouvreDto maneouvre)
        {
            var score = this._score(judge, maneouvre);
            if (!score.HasValue || score.Value >= 1) { return string.Empty; }
            return "needs-attention";
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