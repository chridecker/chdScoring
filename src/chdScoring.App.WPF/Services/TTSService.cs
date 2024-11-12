using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.WPF.Services
{
    public class TTSService : ITTSService
    {
        public Task SpeakAsync(string message,  CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }
    }
}
