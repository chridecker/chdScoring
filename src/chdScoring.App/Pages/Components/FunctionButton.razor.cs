using Microsoft.AspNetCore.Components;

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