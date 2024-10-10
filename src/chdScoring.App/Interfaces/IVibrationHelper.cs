using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.Interfaces
{
     public interface IVibrationHelper
    {
        void Vibrate(TimeSpan duration);
        Task Vibrate(int repeat, TimeSpan duration, CancellationToken cancellationToken = default);
        Task Vibrate(int repeat, TimeSpan duration, TimeSpan breakDuration, CancellationToken cancellationToken = default);
    }
}
