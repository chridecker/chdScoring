using global::Microsoft.AspNetCore.Components;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.App.UI.Constants;
using chd.UI.Base.Components.Base;
using chdScoring.App.UI.Interfaces;

namespace chdScoring.App.UI.Pages
{
    public partial class ScoreCorrection : PageComponentBase<int, int>, IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private IEnumerable<FinishedRoundDto> _roundSets;
        private FinishedRoundDto _selectedRoundSet;
        private RoundDataDto _dto;

        [Inject] ITimerService _timerService { get; set; }
        [Inject] IPilotService _pilotService { get; set; }

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