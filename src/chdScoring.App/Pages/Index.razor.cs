using Microsoft.AspNetCore.Components;
using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Helper;

namespace chdScoring.App.Pages
{

    public partial class Index : IDisposable
    {
        private CancellationTokenSource _cts;
        private CurrentFlight _dto;
        private JudgeDto Judge => this._dto?.Judges.FirstOrDefault(x => x.Id == this._judge);
        private bool _panelDisabled => !this._dto.LeftTime.HasValue ? true : !this.Maneouvres.Any(x => x.Current);

        private IEnumerable<ManeouvreDto> Maneouvres
        {
            get
            {
                if (this._dto?.ManeouvreLst.TryGetValue(this._judge, out var lst) ?? false)
                {
                    return lst;
                }
                return Enumerable.Empty<ManeouvreDto>();
            }
        }

        private int _judge;

        [Inject] private ISettingManager _settingManager { get; set; }
        [Inject] private IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] private IJudgeDataCache _judgeDataCache { get; set; }
        protected override async Task OnInitializedAsync()
        {
            this._cts = new();
            this._judge = await this._settingManager.Judge;
            if (!this._judgeHubClient.IsConnected) { await this._judgeHubClient.StartAsync(this._cts.Token); }
            await this._judgeHubClient.Register(this._judge, this._cts.Token);
            this._judgeHubClient.DataReceived += this._judgeHubClient_DataReceived;
            this._dto = this._judgeDataCache.Data;
            await base.OnInitializedAsync();
        }

        private async void _judgeHubClient_DataReceived(object sender, CurrentFlight e)
        {
            this._dto = e;
            await this.InvokeAsync(this.StateHasChanged);
        }


        public void Dispose()
        {
            this._cts.Cancel();
        }

    }
}