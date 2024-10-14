using chdScoring.Contracts.Enums;

namespace chdScoring.Contracts.Settings
{
    public class DBSettings
    {
        public EDBConnection ConnectionType { get; set; }
        public string ConnectionString { get; set; }
    }
}
