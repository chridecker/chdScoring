using chd.UI.Base.Client.Implementations.Services;
using chd.UI.Base.Components.Base;
using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.App.UI.Pages
{
    public abstract class BaseChdScoringPage : PageComponentBase<int, int>, IDisposable
    {
        private volatile bool _isDisposing = false;
        private CancellationTokenSource _cts = new();
        protected CancellationToken _token => _cts.Token;

        [Inject] protected IPilotService pilotService { get; set; }
        [Inject] protected IModalHandler modalHandler { get; set; }
        [Inject] protected ISettingManager settingManager { get; set; }
        [Inject] protected IScoringService _scoringService { get; set; }
        [Inject] protected IJudgeHubClient _judgeHubClient { get; set; }
        [Inject] protected IJudgeDataCache _judgeDataCache { get; set; }
        [Inject] protected ITimerService _timerService { get; set; }
        [Inject] protected IVibrationHelper _vibrationHelper { get; set; }
        [Inject] protected IDeviceDisplayService _deviceDisplayService { get; set; }



        public void Dispose()
        {
            if (_isDisposing)
            {
                return;
            }

            _isDisposing = true;
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            this._cts.Cancel();
        }
    }
}
