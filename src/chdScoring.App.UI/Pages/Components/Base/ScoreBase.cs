using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Components.Base;
using chd.UI.Base.Contracts.Enum;
using chdScoring.App.UI.Extensions;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Pages.Components.Base
{
    public abstract class ScoreBase : KeyPressListeningComponentBase
    {
        [Inject] protected IVibrationHelper _vibrationHelper { get; set; }

        [Inject] protected ITTSService _tTSService { get; set; }
        [Inject] protected ISettingManager _settingManager { get; set; }
        [Inject] protected IModalHandler _modalHandler { get; set; }

        [Parameter] public Func<Task> ChangeMode { get; set; }
        [Parameter] public Func<SaveScoreDto, Task<bool>> ScoreSaved { get; set; }
        [Parameter] public Func<JudgeDto, PilotDto, int, Task<bool>> ScoresConfirmed { get; set; }
        [Parameter] public int Round { get; set; }

        [Parameter] public PilotDto Pilot { get; set; }

        [Parameter] public JudgeDto Judge { get; set; }

        [Parameter] public ManeouvreDto Maneouvre { get; set; }

        [Parameter] public bool PanelDisabled { get; set; }
        [Parameter] public bool NeedsJudgeConfirmation { get; set; }
        [Parameter] public bool IsConfirmed { get; set; }

        [Parameter] public CancellationToken CancellationToken { get; set; }


        protected abstract decimal? _scoreStartValue();


        protected string _maneouvreText => this.Maneouvre is not null ? $"#{this.Maneouvre?.Id} {this.Maneouvre?.Name}" : " ";
        protected string _scoreValueText => !this._scoreValue.HasValue ? "-" : this._scoreValue.Value < 0 ? "NO" : this._scoreValue.Value == 0 ? "0" : this._scoreValue.Value.ToString("0.#");

        protected decimal? _scoreValue;

        protected async Task ChangePanelMode() => await this.ChangeMode.Invoke();

        protected Task Repeat() => this._tTSService.SpeakAsync(this.Maneouvre?.Name);

        protected virtual async Task Delete()
        {
            this._scoreValue = null;
            await this.InvokeAsync(this.StateHasChanged);
        }


        protected async Task NotObserved()
        {
            this._scoreValue = -99;
            await this._tTSService.SpeakAsync("N O ");
            await this.InvokeAsync(this.StateHasChanged);
        }

        protected async Task ConfirmScores()
        {
            if (!this.PanelDisabled || !this.NeedsJudgeConfirmation || this.IsConfirmed) { return; }
            if (await this._modalHandler.ShowYesNoDialog("Confirm Scores?", this._settingManager.IsiOS) == EDialogResult.Yes)
            {
                await this.ScoresConfirmed?.Invoke(this.Judge, this.Pilot, this.Round);
            }
        }
        protected virtual async Task Save()
        {
            if (this.PanelDisabled) { return; }

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
                this._scoreValue = this._scoreStartValue();
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
