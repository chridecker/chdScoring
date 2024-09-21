using Microsoft.AspNetCore.Components;
using chdScoring.App.Services;
using chdScoring.Contracts.Dtos;

namespace chdScoring.App.Pages
{
    public partial class Settings
    {
        [Inject] private ISettingManager _settingManager { get; set; }
        [Inject] private IMainService _mainService { get; set; }

        private CancellationTokenSource _cts = new CancellationTokenSource();

        private IEnumerable<JudgeDto> _judgeDtos;

        private string _baseAddress = string.Empty;
        private int _judge = 0;
        private bool _isControlCenter = false;

        private bool? isTestSuccess;
        private string testStatusCss => !isTestSuccess.HasValue ? "background-color:grey;" : !isTestSuccess.Value ? "background-color:red;" : "background-color:green;";


        protected override async Task OnInitializedAsync()
        {
            this._baseAddress = await this._settingManager.MainUrl;
            this._judgeDtos = await this._mainService.GetJudges(this._cts.Token);

            this._judge = await this._settingManager.Judge;
            this._isControlCenter = await this._settingManager.IsControlCenter;

            await base.OnInitializedAsync();
        }
        private Task UpdateMainUrl(string setting, ChangeEventArgs e) => this._settingManager.UpdateMainUrl((string)e.Value);

        private Task UpdateJudge(string setting, ChangeEventArgs e) => this._settingManager.UpdateJudge(int.Parse((string)e.Value));
        private Task UpdateIsControlCenter(string setting, ChangeEventArgs e) => this._settingManager.UpdateControlCenter((bool)e.Value);
    }
}
