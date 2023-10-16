using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
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
using chdScoring.Contracts.Dtos;
using chdScoring.App.Services;

namespace chdScoring.App.Pages
{
    public partial class Control : IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private CurrentFlight _dto;
        [Inject] IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] IJudgeDataCache _judgeDataCache { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this._cts = new();
            this._dto = this._judgeDataCache.Data;
            await this._judgeHubClient.Initialize(this._cts.Token);
            await this._judgeHubClient.RegisterControlCenter(this._cts.Token);

            this._judgeHubClient.DataReceived += this._judgeHubClient_DataReceived;

            await base.OnInitializedAsync();
        }
        private async void _judgeHubClient_DataReceived(object sender, CurrentFlight e)
        {
            this._dto = e;
            await this.InvokeAsync(this.StateHasChanged);
        }

        public void Dispose()
        {
            this._judgeHubClient.DataReceived -= this._judgeHubClient_DataReceived;
            this._cts.Cancel();
        }
    }
}