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
using chdScoring.Client;
using chdScoring.Client.Shared;
using chdScoring.Client.Constants;
using chdScoring.Client.Helper;

namespace chdScoring.Client
{
    public partial class Main
    {
        [Inject]private IJudgeHubClient _judgeHubClient { get; set; }
        [Inject]private NavigationManager _navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await this._judgeHubClient.Initialize(this._navigationManager);
            await base.OnInitializedAsync();
        }
    }
}