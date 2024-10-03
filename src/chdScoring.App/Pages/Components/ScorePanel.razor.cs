using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using chdScoring.App.Helper;
using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Handler;
using Blazorise.DeepCloner;
using chdScoring.Contracts.Interfaces;

namespace chdScoring.App.Pages.Components
{
    public partial class ScorePanel : ComponentBase, IAsyncDisposable
    {
        [Inject] private IKeyHandler _keyHandler { get; set; }

        [Inject] private IVibrationHelper _vibrationHelper { get; set; }

        [Inject] private IJSRuntime _jsRuntime { get; set; }



        [Parameter] public Func<SaveScoreDto, Task<bool>> ScoreSaved { get; set; }
        [Parameter] public int Round { get; set; }

        [Parameter] public PilotDto Pilot { get; set; }

        [Parameter] public JudgeDto Judge { get; set; }

        [Parameter] public ManeouvreDto Maneouvre { get; set; }

        [Parameter] public bool PanelDisabled { get; set; }

        [Parameter] public CancellationToken CancellationToken { get; set; }

        private string _scoreValueText => !this._scoreValue.HasValue ? "" : this._scoreValue.Value < 0 ? "NO" : this._scoreValue.Value == 0 ? "0" : this._scoreValue.Value.ToString("#.#");
        private decimal? _scoreValue;

        private string _maneouvreText => this.Maneouvre is not null ? $"#{this.Maneouvre?.Id} {this.Maneouvre?.Name}" : " ";

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

        private async void KeyDown(object sender, KeyboardEventArgs e)
        {
            var t = (int.TryParse(e.Code, out int code), code) switch
            {
                (true, _) when (code is 8 or 46 or 166) => this.Delete(),
                (true, _) when (code is 96 or 48) => this.Calc(10),
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
                _ => Task.CompletedTask
            };
            await t;
        }

        private async Task Delete()
        {
            this._scoreValue = null;
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task Save()
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
        public async Task<bool> SaveScore(int id, int figur, int judge, int round, decimal value, CancellationToken token)
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

        private async Task NotObserved()
        {
            this._scoreValue = -1;
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
            await this.InvokeAsync(this.StateHasChanged);
        }
        public async ValueTask DisposeAsync()
        {
            await this._jsRuntime.InvokeVoidAsync("JsFunctions.removeKeyboardListenerEvent");
            this._keyHandler.KeyDown -= KeyDown;
        }
    }
}