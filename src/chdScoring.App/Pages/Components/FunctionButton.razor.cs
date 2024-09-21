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

namespace chdScoring.App.Pages.Components
{
    public partial class FunctionButton
    {
        [Parameter]
        public bool PanelDisabled { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string CustomStyle { get; set; }

        [Parameter]
        public Func<Task> Execution { get; set; }

        private string _buttonCss => this.PanelDisabled ? "grey" : "";
    }
}