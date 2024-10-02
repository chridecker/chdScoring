using Microsoft.AspNetCore.Components;
using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;
using chdScoring.App.Helper;
using chdScoring.Contracts.Interfaces;
using chd.UI.Base.Contracts.Interfaces.Authentication;
using Blazored.Modal.Services;
using chd.UI.Base.Components.Extensions;

namespace chdScoring.App.Pages
{

    public partial class Index : ComponentBase, IDisposable
    {
        private CancellationTokenSource _cts = new();
        private CurrentFlight _dto;
        private JudgeDto Judge => this._dto?.Judges.FirstOrDefault(x => x.Id == this._judge);
        private bool _panelDisabled => !this._dto.LeftTime.HasValue ? true : !this.Maneouvres.Any(x => x.Current);

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
        [Inject] private IModalService _modal { get; set; }
        [Inject] private IchdScoringProfileService _profileService { get; set; }
        [Inject] private IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] private IJudgeService _judgeService { get; set; }
        [Inject] private IJudgeDataCache _judgeDataCache { get; set; }
        protected override async Task OnInitializedAsync()
        {
            this._profileService.UserChanged += this._profileService_UserChanged;
            await base.OnInitializedAsync();
        }

        private async void _profileService_UserChanged(object sender, chd.UI.Base.Contracts.Dtos.Authentication.UserDto<int, int> e)
        {
            await this.LoadData();
            await this.InvokeAsync(this.StateHasChanged);
        }


        private async Task LoadData()
        {
            if (this._profileService.User?.Id is null)
            {
                this._dto = null;
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
            this._dto = e;
            await this.InvokeAsync(this.StateHasChanged);
        }


        public void Dispose()
        {
            this._cts.Cancel();
        }

    }
}