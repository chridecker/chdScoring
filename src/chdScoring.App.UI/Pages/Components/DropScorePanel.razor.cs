using Blazorise;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Pages.Components.Base;
using chdScoring.Contracts.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Pages.Components
{
    public partial class DropScorePanel : ScoreBase, IDisposable
    {
        [Inject] IKeyHandler _keyHandler { get; set; }

        protected override Task OnInitializedAsync()
        {
            this._keyHandler.KeyInput += this._keyHandler_KeyInput;

            this._scoreValue = 10;


            return base.OnInitializedAsync();
        }

        private async void _keyHandler_KeyInput(object? sender, EKeyInput e)
        {
            if (e is EKeyInput.None) { return; }

            if (e is EKeyInput.Y)
            {
                await this.Calc(-0.5m);
            }
            else if (e is EKeyInput.A)
            {
                await this.Calc(0.5m);
            }
            else if (e is EKeyInput.X)
            {
                await this.Calc(-1m);
            }
            else if (e is EKeyInput.B && this.Judge is not null && this.Pilot is not null && this.Maneouvre is not null)
            {
                await this.Save();
            }

        }


        private async Task Calc(decimal i)
        {
            this._scoreValue += i;
            if (this._scoreValue.Value <= 0) { this._scoreValue = 0; }
            if (this._scoreValue.Value >= 10) { this._scoreValue = 10; }
            this._vibrationHelper.Vibrate(TimeSpan.FromMilliseconds(100));
            await this._tTSService.SpeakAsync(this._scoreValue.Value.ToString("#.#"));
            await this.InvokeAsync(this.StateHasChanged);
        }
        protected override decimal? _scoreStartValue() => 10;

        private async Task Repeat()
        {
            await this._tTSService.SpeakAsync(this.Maneouvre?.Name);
        }

        private async Task Zero()
        {
            this._scoreValue = 0;
            await this._tTSService.SpeakAsync(this._scoreValue.Value.ToString("n0"));
            await this.InvokeAsync(this.StateHasChanged);
        }
        public void Dispose()
        {
            this._keyHandler.KeyInput -= this._keyHandler_KeyInput;
        }
    }
}
