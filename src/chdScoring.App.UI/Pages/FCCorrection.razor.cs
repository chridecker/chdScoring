using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Components.Base;
using chd.UI.Base.Components.Extensions;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chd.UI.Base.Contracts.Enum;
using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Extensions;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Pages.Components;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Components;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using Blazored.Modal;
using chd.UI.Base.Components.General.Search;
using chdScoring.Contracts.Constants;

namespace chdScoring.App.UI.Pages
{

    public partial class FCCorrection : BaseChdScoringPage
    {
        private RoundDataDto _dto;
        private int _zoom;
        private int _judge => 1;

        private IEnumerable<ManeouvreDto> Maneouvres => (this._dto?.ManeouvreLst?.TryGetValue(this._judge, out var lst) ?? false) ? lst : [];

        private ManeouvreDto _current;
        private JudgeDto Judge => this._dto?.Judges.FirstOrDefault(x => x.Id == (this._judge));


        private bool _panelDisabled => this._dto is null || this._current is null;

        [Inject] ITTSService _ttsService { get; set; }
        [Inject] private IJudgeService _judgeService { get; set; }
        [Inject] private IScrollInfoService _scrollInfoService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.Scoring;

            this._zoom = await this.settingManager.GetScoringZoom();

            await base.OnInitializedAsync();
        }

        private async Task ChoosePilot()
        {
            var finishedRounds = await this.pilotService.GetFinishedFlights(this._token);
            var parameters = new ModalParameters
            {
                { nameof(SearchModalComponent<FinishedRoundDto, int>.Items), finishedRounds
                    .OrderByDescending(o=>o.Round.Id)
                    .ThenByDescending(o => o.Start)
                    .ToList() },
                { nameof(SearchModalComponent<FinishedRoundDto, int>.Name),(FinishedRoundDto r)=> $"{r.Pilot.Id} {r.Pilot.Name}, Round {r.Round.Id}, " },
                { nameof(SearchModalComponent<FinishedRoundDto, int>.DisableOrder), true },
            };
            var modalInstance = this.modalHandler.Show<SearchModalComponent<FinishedRoundDto, int>>("PDF erstellen", parameters);

            var result = await modalInstance.Result;
            if (result.Confirmed && result.Data is FinishedRoundDto dto)
            {
                this._dto = await this.pilotService.GetRoundData(dto.Pilot.Id, dto.Round.Id, this._token);
                this._current = this.Maneouvres?.OrderBy(o => o.Id).FirstOrDefault();
                await this.InvokeAsync(this.StateHasChanged);
            }
        }


        private async Task SetCurrent(ManeouvreDto dto)
        {
            this._current = dto;
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task<bool> ScoreSaved(SaveScoreDto dto)
        {
            await this._scoringService.UpdateScore(dto, this._token);

            if (this.Maneouvres.Any(x => x.Id == dto.Figur))
            {
                this.Maneouvres.FirstOrDefault(x => x.Id == dto.Figur).Score = dto.Value;
            }
            if (this.Maneouvres.Any(a => a.Id == (this._current?.Id ?? 0) + 1))
            {
                this._current = this.Maneouvres.FirstOrDefault(a => a.Id == (this._current?.Id ?? 0) + 1);
            }
            await this.InvokeAsync(this.StateHasChanged);
            return true;
        }

    }
}