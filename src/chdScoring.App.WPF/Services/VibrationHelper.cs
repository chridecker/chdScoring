using chdScoring.App.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace chdScoring.App.WPF.Services
{
    public class VibrationHelper : IVibrationHelper
    {
        public void Vibrate(TimeSpan duration)
        {
        }

        public Task Vibrate(int repeat, TimeSpan duration, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

        public Task Vibrate(int repeat, TimeSpan duration, TimeSpan breakDuration, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
    }
}
