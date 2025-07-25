using global::Microsoft.AspNetCore.Components;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.App.UI.Constants;
using chd.UI.Base.Components.Base;
using chdScoring.App.UI.Interfaces;
using chd.UI.Base.Client.Implementations.Services;
using chdScoring.App.UI.Pages.Components;
using Blazored.Modal;

namespace chdScoring.App.UI.Pages
{
    public partial class Control : PageComponentBase<int, int>, IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private RoundDataDto _dto;

        [Inject] IModalHandler _modalHandler { get; set; }
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


        private ManeouvreDto _scoreMan(JudgeDto judge, ManeouvreDto maneouvre) => this._dto.ManeouvreLst[judge.Id].FirstOrDefault(x => x.Id == maneouvre.Id);

        private decimal? _score(JudgeDto judge, ManeouvreDto maneouvre) => _scoreMan(judge, maneouvre)?.Score;


        private string _scoreClass(JudgeDto judge, ManeouvreDto maneouvre)
        {

            var score = this._score(judge, maneouvre);
            if (maneouvre.Histories.Any() || (score.HasValue && score.Value < 1))
            {
                return $"needs-attention is-loading-glow ";
            }
            return string.Empty;
        }

        private string _scoreConfirmed(JudgeDto dto)
        {
            var confirm = this._dto.JudgeConfirms.Any(a => a.Judge == dto.Id);
            return confirm ? "confirmed" : "";
        }

        private async Task OpenHistory(JudgeDto judge, ManeouvreDto maneouvre)
        {
            var man = _scoreMan(judge, maneouvre);
            if (man is not null && maneouvre.Histories.Any())
            {
                var param = new ModalParameters()
                {
                    {nameof(ScoreHistoryComponent.Histories), maneouvre.Histories },
                };

                await this._modalHandler.Show<ScoreHistoryComponent>("Score History", param).Result;
            }
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