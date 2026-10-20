using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Services
{
    public class TTSService(ISettingManager settingManager)
        : BaseTTSService
    {
        protected override async Task Speak(string message, CancellationToken cancellationToken)
        {
            var lang = "de";
            var selectedLang = await settingManager.GetSettingLocal(SettingConstants.SpeechLanguage);
            if (string.IsNullOrEmpty(selectedLang)) { return; }
            var locales = await TextToSpeech.Default.GetLocalesAsync();
            if (!string.IsNullOrWhiteSpace(selectedLang) && locales.Any(x => x.Language.StartsWith(selectedLang.Substring(0, 2))))
            {
                lang = selectedLang;
            }
            var options = new SpeechOptions()
            {
                Locale = locales.FirstOrDefault(x => x.Language.StartsWith(lang)),
                Pitch = 1f,
                Rate = 1f,
                Volume = 1f
            };
            await TextToSpeech.Default.SpeakAsync(message, options, cancelToken: cancellationToken);

        }
    }
}
