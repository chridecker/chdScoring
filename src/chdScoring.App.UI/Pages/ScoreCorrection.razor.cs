using global::Microsoft.AspNetCore.Components;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.App.UI.Constants;
using chd.UI.Base.Components.Base;
using chdScoring.App.UI.Interfaces;
using chd.UI.Base.Contracts.Enum;
using chdScoring.App.UI.Pages.Components;
using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Components.Extensions;
using System.Text.Json.Serialization;

namespace chdScoring.App.UI.Pages
{
    public partial class ScoreCorrection : PageComponentBase<int, int>, IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private IEnumerable<FinishedRoundDto> _roundSets;
        private FinishedRoundDto _selectedRoundSet;
        private RoundDataDto _dto;

        [Inject] IModalHandler _modal { get; set; }
        [Inject] ITimerService _timerService { get; set; }
        [Inject] IPilotService _pilotService { get; set; }
        [Inject] IScoringService _scoringService { get; set; }




        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.ControlCenter;
            this._cts = new();

            this._roundSets = await this._pilotService.GetFinishedFlights(this._cts.Token);

            await base.OnInitializedAsync();
        }
        private async void OnRoundSetChanged(FinishedRoundDto dto)
        {
            this._selectedRoundSet = dto;
            this._dto = await this._pilotService.GetRoundData(dto.Pilot.Id, dto.Round.Id, this._cts.Token);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task OpenEditScoreModal(JudgeDto judge, ManeouvreDto dto)
        {
            var man = this._dto.ManeouvreLst[judge.Id].FirstOrDefault(x => x.Id == dto.Id);
            RenderFragment frag = (__builder) =>
            {
                __builder.OpenComponent<EditScore>(1);
                __builder.AddComponentParameter(2, nameof(EditScore.Dto), man);
                __builder.CloseComponent();
            };
            var change = await this._modal.ShowDialog("Wertung ändern", EDialogButtons.OKCancel, frag);
            if (change == EDialogResult.OK)
            {
                await this._scoringService.UpdateScore(new SaveScoreDto()
                {
                    Pilot = this._dto.Pilot.Id,
                    Figur = man.Id,
                    Judge = judge.Id,
                    Round = this._dto.Round.Id,
                    Value = man.Score.Value,
                    User = this._profileService.User.Id
                }, this._cts.Token);
            }
        }

        private decimal? _score(JudgeDto judge, ManeouvreDto maneouvre) => this._dto.ManeouvreLst[judge.Id].FirstOrDefault(x => x.Id == maneouvre.Id)?.Score;
        private string _scoreClass(JudgeDto judge, ManeouvreDto maneouvre)
        {
            var score = this._score(judge, maneouvre);
            if (!score.HasValue || score.Value >= 1) { return string.Empty; }
            return "needs-attention is-loading-glow ";
        }

        public void Dispose()
        {
            this._cts.Cancel();
        }
    }
}