using chdScoring.App.UI.Constants;
using chdScoring.App.UI.Interfaces;
using chdScoring.App.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chdScoring.App.WPF.Services
{
    public class TTSService : BaseTTSService
    {
        private readonly SpeechSynthesizer _synthesizer;

        public TTSService()
        {
            this._synthesizer = new SpeechSynthesizer();
            this._synthesizer.SetOutputToDefaultAudioDevice();
        }
        protected override Task Speak(string message, CancellationToken cancellationToken) => Task.Run(() =>
        {
            var tcs = new TaskCompletionSource();

            EventHandler<SpeakCompletedEventArgs>? completed = null;

            completed = (_, _) =>
            {
                _synthesizer.SpeakCompleted -= completed;
                tcs.TrySetResult();
            };

            _synthesizer.SpeakCompleted += completed;

            cancellationToken.Register(() =>
            {
                _synthesizer.SpeakAsyncCancelAll();
                tcs.TrySetCanceled(cancellationToken);
            });

            _= _synthesizer.SpeakAsync(message);

            return tcs.Task;
        }, cancellationToken);
    }
}
