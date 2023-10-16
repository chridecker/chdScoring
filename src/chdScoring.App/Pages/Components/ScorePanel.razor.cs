using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using chdScoring.App;
using chdScoring.App.Shared;
using chdScoring.App.Constants;
using chdScoring.App.Helper;
using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Handler;
using static System.Net.Mime.MediaTypeNames;

namespace chdScoring.App.Pages.Components
{
    public partial class ScorePanel : IAsyncDisposable
    {
        [Inject]
        private IMainService _mainService { get; set; }
        [Inject]
        private IKeyHandler _keyHandler { get; set; }

        [Inject]
        private IVibrationHelper _vibrationHelper { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        [Parameter]
        public int Round { get; set; }

        [Parameter]
        public PilotDto Pilot { get; set; }

        [Parameter]
        public JudgeDto Judge { get; set; }

        [Parameter]
        public ManeouvreDto Maneouvre { get; set; }

        [Parameter]
        public bool PanelDisabled { get; set; }

        [Parameter]
        public CancellationToken CancellationToken { get; set; }

        private string _scoreValueText => this._scoreValue.HasValue ? this._scoreValue.Value < 0 ? "NO" : this._scoreValue.Value.ToString("n1") : "";
        private decimal? _scoreValue;


        protected override async Task OnInitializedAsync()
        {
            this._keyHandler.KeyDown += KeyDown;
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await this._jsRuntime.InvokeVoidAsync("JsFunctions.addKeyboardListenerEvent", DotNetObjectReference.Create(_keyHandler));
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async void KeyDown(object sender, KeyboardEventArgs e)
        {
            switch (int.TryParse(e.Code, out var code), code)
            {
                case (true, 8):
                case (true, 46):
                case (true, 166):
                    await this.Del();
                    break;
                case (true, 96):
                case (true, 48):
                    await this.Calc(10);
                    break;
                case (true, 97):
                case (true, 49):
                    await this.Calc(1);
                    break;
                case (true, 98):
                case (true, 50):
                    await this.Calc(2);
                    break;
                case (true, 99):
                case (true, 51):
                    await this.Calc(3);
                    break;
                case (true, 100):
                case (true, 52):
                    await this.Calc(4);
                    break;
                case (true, 101):
                case (true, 53):
                    await this.Calc(5);
                    break;
                case (true, 102):
                case (true, 54):
                    await this.Calc(6);
                    break;
                case (true, 103):
                case (true, 55):
                    await this.Calc(7);
                    break;
                case (true, 104):
                case (true, 56):
                    await this.Calc(8);
                    break;
                case (true, 105):
                case (true, 57):
                    await this.Calc(9);
                    break;
                case (true, 111):
                    await this.NotObserved();
                    break;
                case (true, 13):
                    await this.Save();
                    break;
            }
        }

        private async Task Del()
        {
            this._scoreValue = null;
        }

        private async Task Save()
        {
            if (this._scoreValue.HasValue)
            {
                if (!(await this._mainService.SaveScore(this.Pilot.Id, this.Maneouvre.Id, this.Judge.Id, this.Round, this._scoreValue.Value, this.CancellationToken)))
                {
                    var duration = TimeSpan.FromMilliseconds(200);
                    await this._vibrationHelper.Vibrate(4, duration, this.CancellationToken);
                }
                else
                {
                    this._vibrationHelper.Vibrate(TimeSpan.FromMilliseconds(500));
                    this._scoreValue = null;
                }
            }
        }

        private async Task NotObserved()
        {
            this._scoreValue = -1;
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
        }
        public async ValueTask DisposeAsync()
        {
             await this._jsRuntime.InvokeVoidAsync("JsFunctions.addKeyboardListenerEvent");
            this._keyHandler.KeyDown -= KeyDown;
        }
    }
}