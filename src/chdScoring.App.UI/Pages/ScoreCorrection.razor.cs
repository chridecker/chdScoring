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
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using global::Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace chdScoring.App.UI.Pages
{
    public partial class ScoreCorrection : BaseChdScoringPage
    {
        private RoundDataDto _dto;

        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.ControlCenter;


            await base.OnInitializedAsync();
        }

        private async Task ChoosePilot()
        {
            var finishedRounds = await this.pilotService.GetFinishedFlights();
            var parameters = new ModalParameters
                     {
                         { nameof(SearchModalComponent<FinishedRoundDto, int>.Items), finishedRounds
                         .OrderByDescending(o=>o.Round.Id)
                         .ThenByDescending(o => o.Start)
                         .ToList() },
                         { nameof(SearchModalComponent<FinishedRoundDto, int>.Name),(FinishedRoundDto r)=> $"R{r.Round.Id}, {r.Pilot.Id} {r.Pilot.Name}," },
                         { nameof(SearchModalComponent<FinishedRoundDto, int>.DisableOrder), true },
                     };
            var modalInstance = this.modalHandler.Show<SearchModalComponent<FinishedRoundDto, int>>("PDF erstellen", parameters);

            var result = await modalInstance.Result;
            if (result.Confirmed && result.Data is FinishedRoundDto dto)
            {
                this._dto = await this.pilotService.GetRoundData(dto.Pilot.Id, dto.Round.Id, this._token);
                await this.InvokeAsync(this.StateHasChanged);
            }
        }

        private async Task OpenEditScoreModal(JudgeDto judge, ManeouvreDto dto)
        {
            var man = this._dto.ManeouvreLst[judge.Id].FirstOrDefault(x => x.Id == dto.Id);
            RenderFragment frag = (__builder) =>
            {
                __builder.OpenComponent<EditScore>(1);
                __builder.AddComponentParameter(2, nameof(EditScore.Dto), man);
                __builder.CloseComponent();
            };
            var change = await this.modalHandler.ShowOkCancelDialog("Wertung ändern", this.settingManager.IsiOS, frag);
            if (change == EDialogResult.OK)
            {
                await this._scoringService.UpdateScore(new SaveScoreDto()
                {
                    Pilot = this._dto.Pilot.Id,
                    Figur = man.Id,
                    Judge = judge.Id,
                    Round = this._dto.Round.Id,
                    Value = man.Score.Value,
                    User = this._profileService.User.Id
                }, this._token);
            }
        }

        private decimal? _score(JudgeDto judge, ManeouvreDto maneouvre) => this._dto.ManeouvreLst[judge.Id].FirstOrDefault(x => x.Id == maneouvre.Id)?.Score;
        private string _scoreClass(JudgeDto judge, ManeouvreDto maneouvre)
        {
            var score = this._score(judge, maneouvre);
            if (!score.HasValue || score.Value >= 1) { return string.Empty; }
            return "needs-attention is-loading-glow ";

        }
    }
}