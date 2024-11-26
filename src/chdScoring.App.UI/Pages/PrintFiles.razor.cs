using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Components.Extensions;
using chdScoring.App.UI.Pages.Components;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Components;

namespace chdScoring.App.UI.Pages
{
    public partial class PrintFiles : IDisposable
    {
        [Inject] IModalHandler _modal { get; set; }
        [Inject] IPrintService _printService { get; set; }

        private IEnumerable<PrintPdfDto> _printDtos;
        private CancellationTokenSource _cts = new();

        private string _autoPrintIco => this._autoPrint ? "print-slash" : "bolt-auto";
            
            private bool _autoPrint;


        protected override async Task OnInitializedAsync()
        {
            this._autoPrint = await this._printService.GetAutoPrintSetting(this._cts.Token);
            this._printDtos = await this._printService.GetPdfLst(this._cts.Token);
            await base.OnInitializedAsync();
        }

        private async Task CreatePdf()
        {
            await this._modal.Show<SendToPrintComponent>().Result;

        }

        private async Task ChangeAutoPrint()
        {
            this._autoPrint = await this._printService.ChangeAutoPrint(this._cts.Token);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task Reload()
        {
            this._printDtos = await this._printService.GetPdfLst(this._cts.Token);
            await this.InvokeAsync(this.StateHasChanged);
        }

        public void Dispose()
        {
            this._cts.Cancel();
            this._cts.Dispose();
        }
    }
}