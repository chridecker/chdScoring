using Blazorise;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Pages.Components.Base;
using chdScoring.Contracts.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Pages.Components
{
    public partial class DropScorePanel : ScoreBase
    {
        protected override async Task OnInitializedAsync()
        {
            this._scoreValue = 10;
            await base.OnInitializedAsync();
        }
        protected override decimal? _scoreStartValue() => 10;

        protected override Task KeyDownHandle(KeyboardEventArgs e) => (int.TryParse(e.Code, out int code), code) switch
        {
            (true, _) when (code is 109 or 189 or 40) => this.Calc(-0.5m),
            (true, _) when (code is 107 or 187 or 38) => this.Calc(0.5m),
            (true, _) when (code is 37) => this.Calc(-1m),
            (true, _) when (code is 39) => this.Calc(1m),
            (true, 111) => this.NotObserved(),
            (true, 13) => this.Save(),
            _ => Task.CompletedTask
        };
        private async Task Calc(decimal i)
        {
            this._scoreValue += i;
            if (this._scoreValue.Value <= 0) { this._scoreValue = 0; }
            if (this._scoreValue.Value >= 10) { this._scoreValue = 10; }
            this._vibrationHelper.Vibrate(TimeSpan.FromMilliseconds(100));
            await this.InvokeAsync(this.StateHasChanged);

            await this._tTSService.SpeakAsync(this._scoreValue.Value.ToString("#.#"));
        }
        private async Task Zero()
        {
            this._scoreValue = 0;
            await this._tTSService.SpeakAsync(this._scoreValue.Value.ToString("n0"));
            await this.InvokeAsync(this.StateHasChanged);
        }
    }
}
