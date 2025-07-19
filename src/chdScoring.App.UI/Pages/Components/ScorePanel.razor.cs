using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Pages.Components.Base;
using chdScoring.App.UI.Services;
using chdScoring.Contracts.Dtos;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace chdScoring.App.UI.Pages.Components
{
    public partial class ScorePanel : ScoreBase
    {
        protected override decimal? _scoreStartValue() => null;

        private bool _commaPressed = false;
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
            (true, 111) => this.NotObserved(),
            (true, 13) => this.Save(),
            (true, 110) => this.Comma(),
            _ => Task.CompletedTask
        };



        private async Task Calc(decimal i)
        {
            if (this.PanelDisabled)
            {
                return;
            }

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
            this._vibrationHelper.Vibrate(TimeSpan.FromMilliseconds(100));

            if (this._scoreValue.HasValue)
            {
                await this._tTSService.SpeakAsync(this._scoreValue.Value.ToString("#.#"));
            }
            await this.InvokeAsync(this.StateHasChanged);
        }

        protected async override Task Save()
        {
            this._commaPressed = false;
            await base.Save();
        }

        protected override async Task Delete()
        {
            this._commaPressed = false;
            await base.Delete();
        }

        private async Task Comma()
        {
            this._commaPressed = true;
            await this.InvokeAsync(this.StateHasChanged);
        }
    }
}