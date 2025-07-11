using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Services
{
    public class TTSService : ITTSService
    {
        private readonly ISettingManager _settingManager;

        private CancellationTokenSource _cts;

        public TTSService(ISettingManager settingManager)
        {
            this._settingManager = settingManager;
        }
        public async Task SpeakAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) { return; }
            var lang = "de";
            var selectedLang = await this._settingManager.GetSettingLocal(SettingConstants.SpeechLanguage);
            var locales = await TextToSpeech.Default.GetLocalesAsync();

            if (!string.IsNullOrWhiteSpace(selectedLang) && locales.Any(x => x.Language.StartsWith(selectedLang.Substring(0, 2))))
            {
                lang = selectedLang;
            }

            var options = new SpeechOptions()
            {
                Locale = locales.FirstOrDefault(x => x.Language.StartsWith(lang))
            };

            if (this._cts is not null && !this._cts.IsCancellationRequested)
            {
                this._cts.Cancel();
            }
            this._cts = new();
            await TextToSpeech.Default.SpeakAsync(message, options, cancelToken: this._cts.Token);
            this._cts?.Cancel();
            this._cts = null;
        }
    }
}
