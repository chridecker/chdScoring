using Blazored.Modal;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.App.UI.Pages.Components
{
    public partial class SendToPrintComponent
    {
        [Inject] private IPilotService _pilotService { get; set; }
        [Inject] private IPrintHelper _printHelper { get; set; }
        [CascadingParameter] public BlazoredModalInstance Modal { get; set; }
        IEnumerable<FinishedRoundDto> _finishedRounds { get; set; }


        protected override async Task OnInitializedAsync()
        {
            this._finishedRounds = await this._pilotService.GetFinishedFlights();
            await base.OnInitializedAsync();
        }

        private async Task CreatePdf(int pilot, int round)
        {
            await this._printHelper.PrintRound(pilot, round);
            await this.Modal.CloseAsync();
        }

    }
}
