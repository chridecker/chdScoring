using System;
using System.Collections.Generic;
using System.Text;
using chdScoring.App.UI.Interfaces;

namespace chdScoring.App.UI.Services
{
    public abstract class BaseTTSService : ITTSService
    {
        private CancellationTokenSource? _cts;
        public async Task SpeakAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) { return; }
            if (this._cts is not null && !this._cts.IsCancellationRequested)
            {
                await this._cts.CancelAsync();
                await Task.Delay(5);
            }
            this._cts = new();
            await this.Speak(message, this._cts.Token);
        }

        public async Task CancelAsync()
        {
            if (_cts is not null)
            {
                await this._cts.CancelAsync();
                this._cts = null;
            }
        }

        protected abstract Task Speak(string message, CancellationToken cancellationToken);
    }
}
