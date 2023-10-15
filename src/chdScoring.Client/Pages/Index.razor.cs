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
using chdScoring.Client;
using chdScoring.Client.Shared;
using chdScoring.Client.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.Client.Handler;
using chdScoring.Client.Helper;

namespace chdScoring.Client.Pages
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

        [Inject] private IMainService _mainService { get; set; }
        [Inject] private ISettingManager _settingManager { get; set; }
        [Inject] private IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] private IJudgeDataCache _judgeDataCache { get; set; }
        protected override async Task OnInitializedAsync()
        {
            this._cts = new();
            this._judge = await this._settingManager.Judge;
            await this._judgeHubClient.Register(this._judge, this._cts.Token);
            this._judgeHubClient.DataReceived += this._judgeHubClient_DataReceived;
            //this._dto = await this._mainService.GetCurrentFlight(this._cts.Token);
            this._dto = this._judgeDataCache.Data;
            //this.Reload(this._cts.Token);
            await base.OnInitializedAsync();
        }

        private async void _judgeHubClient_DataReceived(object sender, CurrentFlight e)
        {
            this._dto = e;
            await this.InvokeAsync(this.StateHasChanged);
        }

        private void Reload(CancellationToken cancellationToken) => Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                this._dto = await this._mainService.GetCurrentFlight(cancellationToken);
                await this.InvokeAsync(this.StateHasChanged);
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }, cancellationToken);

        public void Dispose()
        {
            this._cts.Cancel();
        }

    }
}