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

namespace chdScoring.App.UI.Pages
{

    public partial class Index : BaseChdScoringPage
    {
        private CurrentFlight _dto;
        private int _zoom;

        private int? _judge;
        private IEnumerable<JudgeDto> _judges = [];
        private JudgeDto _selectedJudge;

        private float? _currentBrightness;

        private IEnumerable<ManeouvreDto> Maneouvres => (this._dto?.ManeouvreLst?.TryGetValue(this._judge ?? 0, out var lst) ?? false) ? lst : [];

        private ManeouvreDto _current => this.Maneouvres.Where(x => !x.Score.HasValue).OrderBy(o => o.Id).FirstOrDefault();
        private JudgeDto Judge => this._dto?.Judges.FirstOrDefault(x => x.Id == (this._judge ?? 0));


        private bool _panelDisabled => this._dto is null || this._dto.ScoreMode == Contracts.Enums.EScoreMode.FCScore || !this._dto.LeftTime.HasValue || this._dto.LeftTime.Value <= TimeSpan.Zero || this._current is null;
        private bool _needsJudgeConfirm => this._dto is null ? false : this._judge.HasValue && this._dto.JudeConfirmation;
        private bool _isConfirmed => this._needsJudgeConfirm && this._judge.HasValue && this._judge.Value > 0 ? (this._dto?.JudgeConfirms.Any(a => a.Judge == this._judge.Value) ?? false) : true;
        private bool _isAdmin => this._profileService.HasUserRight(RightConstants.AdminId);

        private bool _useJudgeConfirmQuestion = true;


        private float? GetScreenBrightness()
        {
            if ((!this._panelDisabled || !this._isConfirmed) && (this._dto?.LeftTime.HasValue ?? false))
            {
                return 1;
            }
            if (this._judge.HasValue && this._judge.Value > 0
                && this._isConfirmed && this._panelDisabled)
            {
                return 1;
            }

            return this._currentBrightness;
        }

        [Inject] ITTSService _ttsService { get; set; }
        [Inject] private IJudgeService _judgeService { get; set; }
        [Inject] private IScrollInfoService _scrollInfoService { get; set; }
        [Inject] private IBatteryService _batteryService { get; set; }

        [Parameter]
        public int? JudgeId
        {
            get => this._judge;
            set
            {
                this._judge = value;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            this.Title = PageTitleConstants.Scoring;

            this._deviceDisplayService.KeepScreenOn = true;
            this._currentBrightness = this._deviceDisplayService.ScreenBrightness;

            this._useJudgeConfirmQuestion = await this.settingManager.GetUseJudgeConfirmQuestion();
            this._zoom = await this.settingManager.GetScoringZoom();

            this._judgeHubClient.Connected += this._judgeHubClient_Connected;
            this._judgeHubClient.DataReceived += this._judgeHubClient_DataReceived;
            this._profileService.UserChanged += this._profileService_UserChanged;
            this._batteryService.InfoChanged += this._batteryService_InfoChanged;

            await this.LoadData();

            await base.OnInitializedAsync();
        }

        private async void _judgeHubClient_Connected(object? sender, EventArgs e)
        {
            if (this._judge.HasValue && this._judge.Value > 0
               && this._judgeHubClient.IsConnected)
            {
                await this._judgeHubClient.Register(this._judge.Value, this._token);
            }
        }

        private async void OnJudgeChanged(JudgeDto judge)
        {
            this._selectedJudge = judge;
            this._judge = judge.Id;

            if (this._judge.HasValue && this._judge.Value != RightConstants.AdminId)
            {
                await this._judgeHubClient.Register(this._judge.Value, this._token);
            }
            this._judge = judge.Id;
            await this.InvokeAsync(this.StateHasChanged);
            this._judge = judge.Id;
        }

        private async void _judgeHubClient_DataReceived(object sender, CurrentFlight e)
        {
            this._dto = e;
            this._deviceDisplayService.ScreenBrightness = this.GetScreenBrightness();

            await this.InvokeAsync(this.StateHasChanged);
        }

        private async void _profileService_UserChanged(object sender, UserDto<int, int> e)
        {
            await this.LoadData();
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task OpenEditScoreModal(ManeouvreDto dto)
        {
            if (this._dto?.ScoreMode == Contracts.Enums.EScoreMode.FCScore) { return; }
            if (this._isConfirmed) { return; }

            RenderFragment frag = (__builder) =>
            {
                __builder.OpenComponent<EditScore>(1);
                __builder.AddComponentParameter(2, nameof(EditScore.Dto), dto);
                __builder.CloseComponent();
            };
            var change = await this.modalHandler.ShowOkCancelDialog("Change Score", this.settingManager.IsiOS, frag);
            if (change == EDialogResult.OK && dto.Score.HasValue)
            {
                await this._scoringService.UpdateScore(new SaveScoreDto()
                {
                    Pilot = this._dto.Pilot.Id,
                    Figur = dto.Id,
                    Judge = this._judge.Value,
                    Round = this._dto.Round.Id,
                    Value = dto.Score.Value,
                    User = this._profileService.User.Id
                }, this._token);
            }
        }

        private async Task LoadData()
        {
            if (this._profileService.User?.Id is null || this._profileService.HasUserRight(RightConstants.AdminId))
            {
                await this.LoadJudges();
            }

            this._judge ??= this._profileService.User?.Id;
            if (this._judge.HasValue && this._judge.Value > 0 && this._judges.Any())
            {
                this._selectedJudge = this._judges.FirstOrDefault(x => x.Id == this._judge.Value);
            }

            if (!this._judgeHubClient.IsConnected) { this._judgeHubClient.StartAsync(this._token); }

            if (this._judgeHubClient.IsConnected && this._judge.HasValue && this._judge.Value > 0)
            {
                await this._judgeHubClient.Register(this._judge.Value, this._token);
            }

            await this.LoadCurrentData();
        }

        private async Task LoadJudges()
        {
            try
            {
                this._judges = await this._judgeService.GetJudges(this._token);
            }
            catch (Exception ex)
            {
                _ = await this.modalHandler.ShowSmallDialog(ex.Message, EDialogButtons.OK);
            }
        }


        private async Task LoadCurrentData()
        {
            try
            {
                this._dto = this._judgeDataCache.Data ?? await this._judgeService.GetCurrentFlight();
            }
            catch (Exception ex)
            {
                _ = await this.modalHandler.ShowSmallDialog(ex.Message, EDialogButtons.OK);
            }
        }

        private Task<bool> ScoresConfirmed(JudgeDto judge, PilotDto pilotDto, int round)
        {
            return this._scoringService.ConfirmScores(new ConfirmScoresDto()
            {
                Judge = judge.Id,
                Pilot = pilotDto.Id,
                Round = round,
                Time = DateTime.Now
            }, this._token);
        }
        private async Task<bool> ScoreSaved(SaveScoreDto dto)
        {
            if (this._dto?.ScoreMode == Contracts.Enums.EScoreMode.FCScore) { return false; }
            try
            {
                await this._scoringService.SaveScore(dto, this._token);

                if (this.Maneouvres.Any(x => x.Id == dto.Figur))
                {
                    this.Maneouvres.FirstOrDefault(x => x.Id == dto.Figur).Score = dto.Value;
                }

                await this._scrollInfoService.ScrolltoElement("figure-table");

                await this.InvokeAsync(this.StateHasChanged);

                if (this._current is not null)
                {
                    this._ttsService.SpeakAsync(this._current.Name);
                }

                return true;
            }
            catch { await this._ttsService.SpeakAsync("Error"); }
            return false;
        }

        private async void _batteryService_InfoChanged(object? sender, EventArgs e)
        {
            var limit = await this.settingManager.GetSettingLocal<double>(SettingConstants.BatteryWarningLimit);
            limit = limit > 0 ? limit : 15;

            if (this._batteryService.BatteryLevel < limit &&
                !(this._batteryService.Charging.HasValue && this._batteryService.Charging.Value))
            {
                await this._vibrationHelper.Vibrate(5, TimeSpan.FromMilliseconds(200), this._token);
                await this.modalHandler.ShowSmallDialog($"Batterlevel {this._batteryService.BatteryLevel}% kritisch!", EDialogButtons.OK);
            }
        }


        public override void Dispose()
        {
            this._deviceDisplayService.KeepScreenOn = false;
            this._deviceDisplayService.ScreenBrightness = this._currentBrightness;

            this._judgeHubClient.Connected -= this._judgeHubClient_Connected;
            this._profileService.UserChanged -= this._profileService_UserChanged;
            this._batteryService.InfoChanged -= this._batteryService_InfoChanged;
            base.Dispose();
        }
    }
}