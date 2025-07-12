using Blazorise;
using chdScoring.App.UI.Pages.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Pages.Components
{
    public partial class DropScorePanel : ScoreBase
    {
        protected override Task OnInitializedAsync()
        {
            this._scoreValue = 10;
            return base.OnInitializedAsync();
        }

        private async Task Calc(decimal i)
        {
            this._scoreValue += i;
            if (this._scoreValue.Value <= 0) { this._scoreValue = 0; }
            if (this._scoreValue.Value >= 10) { this._scoreValue = 10; }

            await this.InvokeAsync(this.StateHasChanged);
        }
        private async Task Zero()
        {
            this._scoreValue = 0;
            await this.InvokeAsync(this.StateHasChanged);
        }
    }
}
