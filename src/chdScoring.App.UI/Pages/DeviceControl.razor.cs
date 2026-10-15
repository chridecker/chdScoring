using chdScoring.App.UI.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Components;

namespace chdScoring.App.UI.Pages
{
    public partial class DeviceControl : BaseChdScoringPage
    {

        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.DeviceControl;

            await base.OnInitializedAsync();
        }
    }
}
