using Blazored.Modal;
using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Components.Base;
using chd.UI.Base.Components.Extensions;
using chd.UI.Base.Components.General.Search;
using chd.UI.Base.Contracts.Enum;
using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Extensions;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Pages.Components;
using chdScoring.App.UI.Pages.Components.Management;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using static chdScoring.Contracts.Constants.EndpointConstants;

namespace chdScoring.App.UI.Pages
{
    public partial class CompetitionManagement : PageComponentBase<int, int>, IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();

        [Inject] IModalHandler _modal { get; set; }
        [Inject] IVibrationHelper _vibrationHelper { get; set; }
        [Inject] IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] IJudgeDataCache _judgeDataCache { get; set; }
        [Inject] IPilotService _pilotService { get; set; }
        [Inject] ITimerService _timerService { get; set; }
        [Inject] IDatabaseService _databaseService { get; set; }
        [Inject] IPrintHelper _printHelper { get; set; }
        [Inject] IPrintService _printService { get; set; }
        [Inject] ISettingManager _settingManager { get; set; }


        private CurrentFlight _dto;
        private IEnumerable<string> _databaseConnections;
        private string _currentDatabaseConnection;
        private bool _autoPrint;

        private string _autoPrintIco => this._autoPrint ? "print-slash" : "bolt-auto";

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

            this._autoPrint = await this._printService.GetAutoPrintSetting(this._cts.Token);

            await base.OnInitializedAsync();
        }
        private async Task ChangeAutoPrint()
        {
            this._autoPrint = await this._printService.ChangeAutoPrint(this._cts.Token);
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task ChangeStartNumber()
        {
            var pilots = await this._pilotService.GetAllPilots(this._cts.Token);
            if (!pilots.Any()) { return; }
            var parameters = new ModalParameters
                     {
                         { nameof(SearchModalComponent<OpenRoundDto, int>.Items), pilots.Select(s => new OpenRoundDto()
                         {
                             StartNumber = 0,
                             Pilot = s,
                             Round = 0
                         }) },
                         { nameof(SearchModalComponent<OpenRoundDto, int>.RenderType),typeof(NextPilotSearchItem) },
                         { nameof(SearchModalComponent<OpenRoundDto, int>.RenderParameterDict),(OpenRoundDto dto)=> SearchModalComponent<OpenRoundDto,int>.CreateRenderParameterDict(dto,((x)=> nameof(NextPilotSearchItem.Dto),(x)=>x))},
                         { nameof(SearchModalComponent<OpenRoundDto, int>.DisableOrder), true },
                     };
            var modalInstance = this._modal.Show<SearchModalComponent<OpenRoundDto, int>>("Pilot Startnummer ändern", parameters);

            var result = await modalInstance.Result;
            if (result.Confirmed && result.Data is OpenRoundDto dto)
            {
                var val = await this._modal.ShowSmallInputDialog($"Welche Nummer?", this._settingManager.IsiOS, "Neue Startnummer... ");
                if (int.TryParse(val, out int id))
                {
                    await this._pilotService.SetStartnumber(new()
                    {
                        NewStartId = id,
                        Pilot = dto.Pilot
                    });
                }
            }
        }
        private async Task SetBreak()
        {
            if ((this._dto?.LeftTime.HasValue ?? false)
                && (await this._modal.ShowYesNoDialog("Ein Teilnehmer ist derzeit aktiv! Fortfahren?", this._settingManager.IsiOS) != EDialogResult.Yes))
            {
                return;
            }
            await this._pilotService.UnLoadPilot(new LoadPilotDto
            {
                Pilot = this._dto.Pilot.Id,
                Round = this._dto.Round.Id
            }, this._cts.Token);
        }

        private async Task ReflightRound()
        {
            var finishedRounds = await this._pilotService.GetFinishedFlights();
            var parameters = new ModalParameters
                     {
                         { nameof(SearchModalComponent<FinishedRoundDto, int>.Items), finishedRounds.OrderBy(o=>o.Pilot.Name).ThenBy(o=>o.Round.Id).ThenBy(o => o.Start).ToList() },
                         { nameof(SearchModalComponent<FinishedRoundDto, int>.Name),(FinishedRoundDto r)=> $"{r.Pilot.Name}, Runde {r.Round.Id}" },
                         { nameof(SearchModalComponent<FinishedRoundDto, int>.DisableOrder), true },
                     };
            var modalInstance = this._modal.Show<SearchModalComponent<FinishedRoundDto, int>>("Runde wiederholen", parameters);

            var result = await modalInstance.Result;
            if (result.Confirmed && result.Data is FinishedRoundDto dto
                && await this._modal.ShowYesNoDialog($"{dto.Pilot.Name} Runde {dto.Round.Id} wirklich löschen?", this._settingManager.IsiOS) == EDialogResult.Yes)
            {
                await this._pilotService.ReflightRound(new() { Pilot = dto.Pilot.Id, Round = dto.Round.Id }, this._cts.Token);
            }
        }

        private async Task LoadDatabaseData()
        {
            this._databaseConnections = await this._databaseService.GetDatabaseConnections();
            this._currentDatabaseConnection = await this._databaseService.GetCurrentDatabaseConnection();
            var parameters = new ModalParameters
            {
                {nameof(SearchModalComponent<string,int>.Items), this._databaseConnections },
                {nameof(SearchModalComponent<string,int>.RenderType),typeof(DatabaseConnectionRender) },
                {nameof(SearchModalComponent<string,int>.RenderParameterDict),(string db)=> SearchModalComponent<string,int>.CreateRenderParameterDict(db,((x)=> nameof(DatabaseConnectionRender.DatabaseConnection),(x)=>x),((x)=> nameof(DatabaseConnectionRender.IsCurrentDatabaseConnection),(x)=>x == this._currentDatabaseConnection))},
            };
            var modalInstance = this._modal.Show<SearchModalComponent<string, int>>($"Datenbank {this._currentDatabaseConnection}", parameters);

            var result = await modalInstance.Result;
            if (result.Confirmed && result.Data is string choosenDB)
            {
                await this._databaseService.SetDatabaseConnection(choosenDB);
            }

        }

        private async Task SaveRound()
        {
            var avgScore = this._dto?.ManeouvreLst.Values.Select(s => s.Select(ss => ss.Value * (ss.Score ?? 0)).Sum()).Average();

            var duration = this._dto?.Round.Time - this._dto?.LeftTime ?? TimeSpan.Zero;
            if (this._dto is null) { return; }

            if (this._dto.ScoreMode is not Contracts.Enums.EScoreMode.FCScore
                && this._dto.ManeouvreLst.Values.Any(a => a.Any(aa => !aa.Score.HasValue)) || !avgScore.HasValue)
            {
                await this._vibrationHelper.Vibrate(3, TimeSpan.FromMilliseconds(400), this._cts.Token);
                if (await this._modal.ShowOkCancelDialog("Nicht alle Judges haben alle Figuren gewertet!", this._settingManager.IsiOS) != EDialogResult.OK)
                {
                    return;
                }
            }
            var pilot = this._dto.Pilot.Id;
            var round = this._dto.Round.Id;
            if (await this._timerService.SaveRound(new SaveRoundDto
            {
                Score = avgScore ?? 0,
                Pilot = this._dto.Pilot.Id,
                Round = this._dto.Round.Id,
                Duration = duration

            }, this._cts.Token))
            {
                this._vibrationHelper.Vibrate(TimeSpan.FromSeconds(0.5));
                if (await this._modal.ShowYesNoDialog($"Create PDF?", this._settingManager.IsiOS) == EDialogResult.Yes)
                {
                    await this._printHelper.PrintRound(pilot, round);
                }
            }
            else
            {
                await this._vibrationHelper.Vibrate(3, TimeSpan.FromSeconds(0.4), this._cts.Token);
                await this._modal.ShowSmallDialog("Beim Speichern der Runde ist ein Fehler aufgetreten!", EDialogButtons.OK);
            }
        }


        private async Task PrintPdf()
        {
            var printDtos = await this._printService.GetPdfLst(this._cts.Token);
            if (!printDtos.Any()) { return; }
            var parameters = new ModalParameters
                     {
                         { nameof(SearchModalComponent<PrintPdfDto, int>.Items), printDtos.OrderBy(o=>o.CreationTime).ToList() },
                         { nameof(SearchModalComponent<PrintPdfDto, int>.RenderType),typeof(PrintPdfComponent)},
                         { nameof(SearchModalComponent<PrintPdfDto, int>.RenderParameterDict),(PrintPdfDto dto)=> SearchModalComponent<PrintPdfDto,int>.CreateRenderParameterDict(dto,((x)=> nameof(PrintPdfComponent.Dto),(x)=>x),((x)=> nameof(PrintPdfComponent.Token),(x)=> this._cts.Token))},
                        { nameof(SearchModalComponent<PrintPdfDto, int>.DisableOrder), true },
                     };
            var modalInstance = this._modal.Show<SearchModalComponent<PrintPdfDto, int>>("PDF erstellen", parameters);

            var result = await modalInstance.Result;
            if (result.Confirmed && result.Data is PrintPdfDto dto)
            {
                _ = await this._printService.AddToPrintCache(dto, this._cts.Token);
            }
        }

        private async Task CreatePdf()
        {
            var finishedRounds = await this._pilotService.GetFinishedFlights();
            var parameters = new ModalParameters
                     {
                         { nameof(SearchModalComponent<FinishedRoundDto, int>.Items), finishedRounds
                         .OrderByDescending(o=>o.Round.Id)
                         .ThenByDescending(o => o.Start)
                         .ToList() },
                         { nameof(SearchModalComponent<FinishedRoundDto, int>.Name),(FinishedRoundDto r)=> $"R{r.Round.Id}, {r.Pilot.Id} {r.Pilot.Name}," },
                         { nameof(SearchModalComponent<FinishedRoundDto, int>.DisableOrder), true },
                     };
            var modalInstance = this._modal.Show<SearchModalComponent<FinishedRoundDto, int>>("PDF erstellen", parameters);

            var result = await modalInstance.Result;
            if (result.Confirmed && result.Data is FinishedRoundDto dto)
            {
                await this._printHelper.PrintRound(dto.Pilot.Id, dto.Round.Id);
            }
        }
        private async Task CalculateTBL()
        {
            var pilots = await this._pilotService.GetOpenRound(this._dto?.Round?.Id, this._cts.Token);
            if (pilots.Any())
            {
                await this._modal.ShowSmallDialog($"Es sind noch offene Wertungsflüge in der aktuellen Runde!", EDialogButtons.OK);
                return;
            }
            var round = await this._timerService.GetFinishedRound(this._cts.Token);
            await this._timerService.CalculateRoundTBL(new CalcRoundDto()
            {
                Round = round
            }, this._cts.Token);
        }


        private async Task LoadNextPilot(bool takeFirst = false)
        {
            var pilots = await this._pilotService.GetOpenRound(this._dto?.Round?.Id, this._cts.Token);
            if (pilots.Any())
            {
                OpenRoundDto dto = takeFirst ? pilots.OrderBy(o => o.StartNumber).FirstOrDefault() : await this.ChoosePilotModal(pilots);
                if (dto != null && await this._pilotService.SetPilotActive(new LoadPilotDto
                {
                    Pilot = dto.Pilot.Id,
                    Round = dto.Round
                }, this._cts.Token))
                {
                    this._vibrationHelper.Vibrate(TimeSpan.FromSeconds(0.5));
                }
            }
        }

        private async Task<OpenRoundDto> ChoosePilotModal(IEnumerable<OpenRoundDto> pilots)
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
                return dto;
            }
            return null;
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