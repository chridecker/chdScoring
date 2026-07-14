using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Components.Base;
using chd.UI.Base.Contracts.Enum;
using chdScoring.App.UI.Extensions;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Services;
using chdScoring.Contracts.Dtos;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace chdScoring.App.UI.Pages.Components
{
    public partial class NumPadScorePanel : KeyPressListeningComponentBase
    {
        [Inject] IVibrationHelper _vibrationHelper { get; set; }

        [Inject] ITTSService _tTSService { get; set; }
        [Inject] ISettingManager _settingManager { get; set; }
        [Inject] IModalHandler _modalHandler { get; set; }

        [Parameter] public Func<SaveScoreDto, Task<bool>> ScoreSaved { get; set; }
        [Parameter] public Func<JudgeDto, PilotDto, int, Task<bool>> ScoresConfirmed { get; set; }
        [Parameter] public int Round { get; set; }

        [Parameter] public PilotDto Pilot { get; set; }

        [Parameter] public JudgeDto Judge { get; set; }

        [Parameter] public ManeouvreDto Maneouvre { get; set; }

        [Parameter] public bool PanelDisabled { get; set; }
        [Parameter] public bool NeedsJudgeConfirmation { get; set; }
        [Parameter] public bool IsConfirmed { get; set; }
        [Parameter] public bool UseJudgeConfirmQuestion { get; set; } = true;

        [Parameter] public CancellationToken CancellationToken { get; set; }

        private decimal? _scoreStartValue() => null;
        private bool _commaPressed = false;

        private string _scoreValueText => !this._scoreValue.HasValue ? "-" : this._scoreValue.Value < 0 ? "NO" : this._scoreValue.Value == 0 ? "0" : this._scoreValue.Value.ToString("0.#");

        private decimal? _scoreValue;

        protected override Task KeyDownHandle(KeyboardEventArgs e) => (int.TryParse(e.Code, out int code), code) switch
        {
            (true, _) when (code is 8 or 46 or 166) => this.Delete(),
            (true, _) when (code is 96 or 48) => this.Calc(0),
            (true, _) when (code is 97 or 49) => this.Calc(1),
            (true, _) when (code is 98 or 50) => this.Calc(2),
            (true, _) when (code is 99 or 51) => this.Calc(3),
            (true, _) when (code is 100 or 52) => this.Calc(4),
            (true, _) when (code is 101 or 53) => this.Calc(5),
            (true, _) when (code is 102 or 54) => this.Calc(6),
            (true, _) when (code is 103 or 55) => this.Calc(7),
            (true, _) when (code is 104 or 56) => this.Calc(8),
            (true, _) when (code is 105 or 57) => this.Calc(9),
            (true, _) when (code is 109 or 189 or 40) => this.Calc(-0.5m, true),
            (true, _) when (code is 107 or 187 or 38) => this.Calc(0.5m, true),
            (true, _) when (code is 37) => this.Calc(-1m, true),
            (true, _) when (code is 39) => this.Calc(1m, true),
            (true, _) when (code is 13 or 32) => this.Save(),
            (true, 111) => this.NotObserved(),
            (true, 110) => this.Comma(),
            _ => Task.CompletedTask
        };
        private string _pilotName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Pilot?.Name)
                    && this.Pilot.Name.Split(' ').Length > 1
                    && this.Pilot.Name.Split(' ')[1].Length > 10)
                {
                    return this.Pilot.Name.Split(' ')[0].Substring(0, 1) + ". " + this.Pilot.Name.Split(' ')[1];
                }
                return this.Pilot?.Name;
            }
        }


        private async Task Calc(decimal i, bool useDrop = false)
        {
            if (this.PanelDisabled) { return; }

            if (useDrop)
            {
                if (!this._scoreValue.HasValue)
                {
                    this._scoreValue = 10;
                }
                this._scoreValue += i;
                if (this._scoreValue.Value <= 0) { this._scoreValue = 0; }
                if (this._scoreValue.Value >= 10) { this._scoreValue = 10; }
            }
            else
            {
                if (this._scoreValue.HasValue && _scoreValue == 1 && i == 10)
                {
                    this._scoreValue = 10;
                    this._commaPressed = false;
                }
                else if (this._scoreValue.HasValue && _scoreValue == 1 && i == 0 && !this._commaPressed)
                {
                    this._scoreValue = 10;
                    this._commaPressed = false;
                }
                else if (this._scoreValue.HasValue && _scoreValue == i)
                {
                    this._scoreValue += 0.5m;
                    this._commaPressed = false;
                }
                else if (this._scoreValue.HasValue && _scoreValue < 10 && i == 5 && this._commaPressed)
                {
                    this._scoreValue += i / 10;
                    this._commaPressed |= false;
                }
                else if (this._scoreValue.HasValue && _scoreValue != i && !this._commaPressed)
                {
                    this._scoreValue = i;
                    this._commaPressed = false;
                }
                else if (!this._scoreValue.HasValue)
                {
                    this._scoreValue = i;
                }
            }
            this._vibrationHelper.Vibrate(TimeSpan.FromMilliseconds(100));
            await this.InvokeAsync(this.StateHasChanged);

            if (this._scoreValue.HasValue)
            {
                await this._tTSService.SpeakAsync(this._scoreValue.Value.ToString("#.#"));
            }
            await this.InvokeAsync(this.StateHasChanged);
        }

        private Task Repeat() => this._tTSService.SpeakAsync(this.Maneouvre?.Name);

        private async Task Save()
        {
            if (this.PanelDisabled && this.NeedsJudgeConfirmation && !this.IsConfirmed)
            {
                await this.ConfirmScores();
            }
            else if (this.PanelDisabled)
            {
                return;
            }

            this._commaPressed = false;
            if (this.PanelDisabled) { return; }

            if (this._scoreValue.HasValue)
            {
                if (!(await this.SaveScore(this.Pilot.Id, this.Maneouvre.Id, this.Judge.Id, this.Round, this._scoreValue.Value, this.CancellationToken)))
                {
                    await this._vibrationHelper.Vibrate(4, TimeSpan.FromMilliseconds(200), this.CancellationToken);
                }
                else
                {
                    this._vibrationHelper.Vibrate(TimeSpan.FromMilliseconds(300));
                }
                this._scoreValue = this._scoreStartValue();
            }
            await this.InvokeAsync(this.StateHasChanged);
        }


        private async Task ConfirmScores()
        {
            if (!this.PanelDisabled || !this.NeedsJudgeConfirmation || this.IsConfirmed) { return; }
            if (!this.UseJudgeConfirmQuestion || (await this._modalHandler.ShowYesNoDialog("Confirm Scores?", this._settingManager.IsiOS) == EDialogResult.Yes))
            {
                await this.ScoresConfirmed?.Invoke(this.Judge, this.Pilot, this.Round);
            }
        }

        private async Task<bool> SaveScore(int id, int figur, int judge, int round, decimal value, CancellationToken token)
        {
            var dto = new SaveScoreDto
            {
                Pilot = id,
                Figur = figur,
                Judge = judge,
                Round = round,
                Value = value,
            };
            return await this.ScoreSaved.Invoke(dto);
        }

        private async Task Delete()
        {
            this._commaPressed = false;
            this._scoreValue = null;
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task Comma()
        {
            this._commaPressed = true;
            await this.InvokeAsync(this.StateHasChanged);
        }
        private async Task NotObserved()
        {
            this._scoreValue = -99;
            await this.InvokeAsync(this.StateHasChanged);
            await this._tTSService.SpeakAsync("N O ");
            await this.InvokeAsync(this.StateHasChanged);
        }
    }
}