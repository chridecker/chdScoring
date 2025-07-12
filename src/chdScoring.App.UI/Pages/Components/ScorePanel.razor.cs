using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using chdScoring.Contracts.Dtos;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Pages.Components.Base;

namespace chdScoring.App.UI.Pages.Components
{
    public partial class ScorePanel : ScoreBase
    {
       

        private async Task Delete()
        {
            this._scoreValue = null;
            await this.InvokeAsync(this.StateHasChanged);
        }

        

        private async Task Calc(decimal i)
        {
            if (this.PanelDisabled)
            {
                return;
            }

            if (this._scoreValue.HasValue && _scoreValue == 1 && i == 10)
            {
                this._scoreValue = 10;
            }
            else if (this._scoreValue.HasValue && _scoreValue < 10 && i == 5)
            {
                this._scoreValue += i / 10;
            }
            else if (!this._scoreValue.HasValue)
            {
                this._scoreValue = i;
            }
            if (this._scoreValue.HasValue)
            {
                await this._tTSService.SpeakAsync(this._scoreValue.Value.ToString("#.#"));
            }

            await this.InvokeAsync(this.StateHasChanged);
        }
    }
}