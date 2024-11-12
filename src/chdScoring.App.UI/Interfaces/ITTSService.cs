using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Interfaces
{
    public interface ITTSService
    {
        Task SpeakAsync(string message,CancellationToken cancellation = default);
    }
}
