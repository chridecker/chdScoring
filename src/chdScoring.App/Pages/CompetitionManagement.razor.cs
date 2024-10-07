using Blazored.Modal;
using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Components.Base;
using chd.UI.Base.Components.Extensions;
using chd.UI.Base.Components.General.Search;
using chd.UI.Base.Contracts.Enum;
using chdScoring.App.Constants;
using chdScoring.App.Helper;
using chdScoring.App.Pages.Components;
using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Components;

namespace chdScoring.App.Pages
{
    public partial class CompetitionManagement : PageComponentBase<int, int>, IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private CurrentFlight _dto;

        [Inject] IModalHandler _modal { get; set; }
        [Inject] IVibrationHelper _vibrationHelper { get; set; }
        [Inject] IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] IJudgeDataCache _judgeDataCache { get; set; }
        [Inject] IPilotService _pilotService { get; set; }
        [Inject] ITimerService _timerService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.CompetitionManagement;
            this._cts = new();
            this._dto = this._judgeDataCache.Data;
            if (!this._judgeHubClient.IsConnected)
            {
                await this._judgeHubClient.StartAsync(this._cts.Token);

            }
            await this._judgeHubClient.RegisterControlCenter(this._cts.Token);

            this._judgeHubClient.DataReceived += this._judgeHubClient_DataReceived;

            await base.OnInitializedAsync();
        }


        private async Task SaveRound()
        {
            if (await this._timerService.SaveRound(new SaveRoundDto
            {

            }, this._cts.Token))
            {
                this._vibrationHelper.Vibrate(TimeSpan.FromSeconds(0.5));
            }
            else
            {
                await this._vibrationHelper.Vibrate(3, TimeSpan.FromSeconds(0.3), this._cts.Token);
                await this._modal.ShowDialog("Beim Speichern der Runde ist ein Fehler aufgetreten!", chd.UI.Base.Contracts.Enum.EDialogButtons.OK);
            }
        }

        private async Task LoadNextPilot()
        {
            try
            {
                var pilots = await this._pilotService.GetOpenRound(this._dto?.Round?.Id, this._cts.Token);
                if (pilots.Any())
                {

                    var parameters = new ModalParameters
                     {
                         { nameof(SearchModalComponent<OpenRoundDto, int>.Items), pilots },
                            
                         { nameof(SearchModalComponent<OpenRoundDto, int>.RenderType),typeof(NextPilotSearchItem) },
                         { nameof(SearchModalComponent<OpenRoundDto, int>.RenderParameterDict),(OpenRoundDto dto)=> SearchModalComponent<OpenRoundDto,int>.CreateRenderParameterDict(dto,((x)=> nameof(NextPilotSearchItem.Dto),(x)=>x))},
                         { nameof(SearchModalComponent<OpenRoundDto, int>.DisableOrder), true },
                     };
                    var modalInstance = this._modal.Show<SearchModalComponent<OpenRoundDto, int>>("Nächster Pilot", parameters);

                    var result = await modalInstance.Result;
                    if (result.Confirmed && result.Data is OpenRoundDto dto)
                    {
                        if (await this._pilotService.SetPilotActive(new LoadPilotDto
                        {
                            Pilot = dto.Pilot.Id,
                            Round = dto.Round
                        }, this._cts.Token))
                        {
                            this._vibrationHelper.Vibrate(TimeSpan.FromSeconds(0.5));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                await this._vibrationHelper.Vibrate(3, TimeSpan.FromSeconds(0.3), this._cts.Token);
                await this._modal.ShowDialog(ex.Message, EDialogButtons.OK);
                return;
            }

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