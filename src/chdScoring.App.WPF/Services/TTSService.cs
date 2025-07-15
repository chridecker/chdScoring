using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chdScoring.App.WPF.Services
{
    public class TTSService : ITTSService
    {
        private readonly SpeechSynthesizer _synthesizer;
        private readonly ISettingManager _settingManager;
        private CancellationTokenSource _cts;

        public TTSService(ISettingManager settingManager)
        {
            this._synthesizer = new SpeechSynthesizer();
            this._synthesizer.SetOutputToDefaultAudioDevice();
            this._settingManager = settingManager;
        }
        public async Task SpeakAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) { return; }
            if (this._cts is not null && !this._cts.IsCancellationRequested)
            {
                this._cts.Cancel();
                await Task.Delay(100);
            }
            this._cts = new();
            await this.Speak(message, this._cts.Token);
            this._cts?.Cancel();
            this._cts = null;
        }

        private Task Speak(string message, CancellationToken cancellationToken) => Task.Run(() =>
        {
            this._synthesizer.Speak(message);
        }, cancellationToken);
    }
}
