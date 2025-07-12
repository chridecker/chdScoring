using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Pages.Components.Base
{
    public abstract class ScoreBase : ComponentBase
    {
        [Inject] protected IVibrationHelper _vibrationHelper { get; set; }

        [Inject] protected ITTSService _tTSService { get; set; }

        [Parameter] public Func<SaveScoreDto, Task<bool>> ScoreSaved { get; set; }
        [Parameter] public int Round { get; set; }

        [Parameter] public PilotDto Pilot { get; set; }

        [Parameter] public JudgeDto Judge { get; set; }

        [Parameter] public ManeouvreDto Maneouvre { get; set; }

        [Parameter] public bool PanelDisabled { get; set; }

        [Parameter] public CancellationToken CancellationToken { get; set; }

        protected string _scoreValueText => !this._scoreValue.HasValue ? "" : this._scoreValue.Value < 0 ? "NO" : this._scoreValue.Value == 0 ? "0" : this._scoreValue.Value.ToString("#.#");
        protected decimal? _scoreValue;

        protected string _maneouvreText => this.Maneouvre is not null ? $"#{this.Maneouvre?.Id} {this.Maneouvre?.Name}" : " ";

        protected async Task NotObserved()
        {
            this._scoreValue = -1;
            await this.InvokeAsync(this.StateHasChanged);
        }

        protected async Task Save()
        {
            if (this._scoreValue.HasValue)
            {
                if (!(await this.SaveScore(this.Pilot.Id, this.Maneouvre.Id, this.Judge.Id, this.Round, this._scoreValue.Value, this.CancellationToken)))
                {
                    await this._vibrationHelper.Vibrate(4, TimeSpan.FromMilliseconds(200), this.CancellationToken);
                }
                else
                {
                    this._vibrationHelper.Vibrate(TimeSpan.FromMilliseconds(500));
                }
                this._scoreValue = null;
            }
            await this.InvokeAsync(this.StateHasChanged);
        }
        private async Task<bool> SaveScore(int id, int figur, int judge, int round, decimal value, CancellationToken token)
        {
            var dto = new SaveScoreDto
            {
                Pilot = id,
                Figur = figur,
                Judge = judge,
                Round = round,
                Value = value
            };
            return await this.ScoreSaved?.Invoke(dto);
        }
    }
}
