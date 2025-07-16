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
    public partial class DropScorePanel : ScoreBase, IDisposable
    {
        [Inject] IKeyHandler _keyHandler { get; set; }
        [Inject] IJoystickHandler _joystickHandler { get; set; }

        protected override Task OnInitializedAsync()
        {
            this._keyHandler.KeyInput += this._keyHandler_KeyInput;
            this._joystickHandler.Motion += this._joystickHandler_Motion;

            this._scoreValue = 10;


            return base.OnInitializedAsync();
        }
        protected override decimal? _scoreStartValue() => 10;

        protected override  Task KeyDownHandle(KeyboardEventArgs e) => (int.TryParse(e.Code, out int code), code) switch
        {
            (true,_) when (code is 109 or 189 or 40)=> this.Calc(-0.5m),
            (true,_) when (code is 107 or 187 or 38)=> this.Calc(0.5m),
            (true,_) when (code is 37)=> this.Calc(-1m),
            (true,_) when (code is 39)=> this.Calc(1m),
            (true,111) => this.NotObserved(),
            (true,13) => this.Save(),
            _ => Task.CompletedTask
        };


        private async void _joystickHandler_Motion(object? sender, EJoystickMotionDirection e)
        {
            if (this.PanelDisabled) { return; }
            await this.HandleTask(e);
        }

        private Task HandleTask(EJoystickMotionDirection motion) => motion switch
        {
            EJoystickMotionDirection.Left => this.Calc(0.5m),
            EJoystickMotionDirection.Right => this.Calc(-0.5m),
            EJoystickMotionDirection.Up => this.Calc(1m),
            EJoystickMotionDirection.Down => this.Calc(-1m),
            _ => Task.CompletedTask,

        };

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
            await this.InvokeAsync(this.StateHasChanged);

            await this._tTSService.SpeakAsync(this._scoreValue.Value.ToString("#.#"));
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
            this._joystickHandler.Motion -= this._joystickHandler_Motion;
        }
    }
}
