using System;

namespace chdScoring.Contracts.Settings
{
    public class AppSettings
    {
        public TimeSpan RefreshInterval { get; set; }
        public TimeSpan SendInterval { get; set; }
    }
}
