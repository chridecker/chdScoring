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
    public partial class Control : BaseChdScoringPage
    {
        private RoundDataDto _dto;


        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.ControlCenter;
            this._deviceDisplayService.KeepScreenOn = true;

            this._dto = this._judgeDataCache.Data;
            if (!this._judgeHubClient.IsConnected)
            {
                await this._judgeHubClient.StartAsync(this._token);

            }
            await this._judgeHubClient.RegisterControlCenter(this._token);

            this._judgeHubClient.DataReceived += this._judgeHubClient_DataReceived;

            await base.OnInitializedAsync();
        }


        private ManeouvreDto _scoreMan(JudgeDto judge, ManeouvreDto maneouvre) => this._dto.ManeouvreLst[judge.Id].FirstOrDefault(x => x.Id == maneouvre.Id);

        private decimal? _score(JudgeDto judge, ManeouvreDto maneouvre) => _scoreMan(judge, maneouvre)?.Score;


        private string _scoreClass(JudgeDto judge, ManeouvreDto maneouvre)
        {
            var man = _scoreMan(judge, maneouvre);
            var score = this._score(judge, maneouvre);
            if (man.Histories.Any() || (score.HasValue && score.Value < 1 && score.Value >= 0))
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
            if (man is not null && man.Histories.Any())
            {
                var param = new ModalParameters()
                {
                    {nameof(ScoreHistoryComponent.Histories), man.Histories },
                };

                await this.modalHandler.Show<ScoreHistoryComponent>("Score History", param).Result;
            }
        }

        private async void _judgeHubClient_DataReceived(object sender, CurrentFlight e)
        {
            this._dto = e;
            await this.InvokeAsync(this.StateHasChanged);
        }


        public override void Dispose()
        {
            this._deviceDisplayService.KeepScreenOn = false;
            this._judgeHubClient.DataReceived -= this._judgeHubClient_DataReceived;
            base.Dispose();
        }
    }
}