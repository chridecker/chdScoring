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

        public TTSService(ISettingManager settingManager)
        {
            this._settingManager = settingManager;
        }
        public async Task SpeakNowAsync(string message, string lang = "de", CancellationToken cancellation = default)
        {
            if (string.IsNullOrWhiteSpace(message)) { return; }
            IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();
            var options = new SpeechOptions()
            {
                Locale = locales.FirstOrDefault(x => x.Language.StartsWith(lang))
            };

            await TextToSpeech.Default.SpeakAsync(message, options, cancelToken: cancellation);
        }
    }
}
