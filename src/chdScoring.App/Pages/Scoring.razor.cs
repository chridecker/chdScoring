using Microsoft.AspNetCore.Components;
using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Helper;
using chdScoring.Contracts.Interfaces;
using Blazored.Modal.Services;
using chd.UI.Base.Components.Extensions;
using chd.UI.Base.Contracts.Dtos.Authentication;
using chd.UI.Base.Components.Base;
using chdScoring.App.Constants;
using chd.UI.Base.Client.Implementations.Services;
using System.Collections.Concurrent;
using chd.UI.Base.Contracts.Extensions;
using chd.UI.Base.Contracts.Enum;
using chdScoring.App.Pages.Components;

namespace chdScoring.App.Pages
{

    public partial class Scoring : PageComponentBase<int, int>, IDisposable
    {
        private CancellationTokenSource _cts = new();
        private CurrentFlight _dto;
        private ManeouvreDto _current => this.Maneouvres.Where(x => !x.Score.HasValue).OrderBy(o => o.Id).FirstOrDefault();

        private JudgeDto Judge => this._dto?.Judges.FirstOrDefault(x => x.Id == this._judge);
        private bool _panelDisabled => this._dto is null || !this._dto.LeftTime.HasValue || this._dto.LeftTime.Value <= TimeSpan.Zero ? true : this._current is null;
        private bool _scrolledManually = false;

        private IEnumerable<ManeouvreDto> Maneouvres
        {
            get
            {
                if (this._dto?.ManeouvreLst.TryGetValue(this._judge, out var lst) ?? false)
                {
                    return lst;
                }
                return Enumerable.Empty<ManeouvreDto>();
            }
        }

        private int _judge;

        private BlockingCollection<SaveScoreDto> _unsavedScores = new BlockingCollection<SaveScoreDto>();

        [Inject] private IModalService _modal { get; set; }
        [Inject] private IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] private IJudgeService _judgeService { get; set; }
        [Inject] private IScoringService _scoringService { get; set; }
        [Inject] private IJudgeDataCache _judgeDataCache { get; set; }
        [Inject] private IScrollInfoService _scrollInfoService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.Scoring;
            this._scrollInfoService.OnScroll += this._scrollInfoService_OnScroll;
            this._profileService.UserChanged += this._profileService_UserChanged;

            await this.LoadData();
            this.ResendUnsavedScore(this._cts.Token);

            await base.OnInitializedAsync();
        }

        private void _scrollInfoService_OnScroll(object sender, int e)
        {
            this._scrolledManually = true;
        }

        private async void _profileService_UserChanged(object sender, UserDto<int, int> e)
        {
            await this.LoadData();
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task OpenEditScoreModal(ManeouvreDto dto)
        {
            RenderFragment frag = (__builder) =>
            {
                __builder.OpenComponent<EditScore>(1);
                __builder.AddComponentParameter(2, nameof(EditScore.Dto), dto);
                __builder.CloseComponent();
            };
            var change = await this._modal.ShowDialog("Wertung ändern", EDialogButtons.OKCancel, frag);
            if (change == EDialogResult.OK)
            {
                await this._scoringService.UpdateScore(new SaveScoreDto()
                {
                    Pilot = this._dto.Pilot.Id,
                    Figur = dto.Id,
                    Judge = this._judge,
                    Round = this._dto.Round.Id,
                    Value = dto.Score.Value
                }, this._cts.Token);
            }
        }

        private async Task LoadData()
        {
            if (this._profileService.User?.Id is null)
            {
                return;
            }
            this._judge = this._profileService.User.Id;
            if (!this._judgeHubClient.IsConnected) { await this._judgeHubClient.StartAsync(this._cts.Token); }
            await this._judgeHubClient.Register(this._judge, this._cts.Token);
            this._judgeHubClient.DataReceived += this._judgeHubClient_DataReceived;
            this._dto = this._judgeDataCache.Data ?? await this._judgeService.GetCurrentFlight();
        }

        private async void _judgeHubClient_DataReceived(object sender, CurrentFlight e)
        {
            if (this._dto.Pilot != e.Pilot || this._dto.Round.Id != e.Round.Id)
            {
                while (this._unsavedScores.TryTake(out _)) { }
            }
            this._dto = e;
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task<bool> ScoreSaved(SaveScoreDto dto)
        {
            try
            {
                this._scrolledManually = false;
                this._unsavedScores.Add(dto);
                this.Maneouvres.FirstOrDefault(x => x.Id == dto.Figur).Score = dto.Value;

                await this._scrollInfoService.ScrolltoElement("figure-table");
                this._scrolledManually = false;

                await this.InvokeAsync(this.StateHasChanged);
                return true;
            }
            catch { }
            return false;
        }


        private void ResendUnsavedScore(CancellationToken cancellationToken) => Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!this._unsavedScores.Any())
                {
                    var dto = this._unsavedScores.Take(cancellationToken);
                    try
                    {
                        await this._scoringService.SaveScore(dto, cancellationToken);
                    }
                    catch { this._unsavedScores.Add(dto); }
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                }
            }
        }, cancellationToken);

        public void Dispose()
        {
            this._profileService.UserChanged -= this._profileService_UserChanged;
            this._scrollInfoService.OnScroll -= this._scrollInfoService_OnScroll;
            this._cts.Cancel();
        }
    }
}