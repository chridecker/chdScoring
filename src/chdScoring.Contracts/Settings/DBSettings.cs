using chdScoring.Contracts.Enums;
using System.Collections.Generic;

namespace chdScoring.Contracts.Settings
{
    public class DBSettings
    {
        public EDBConnection ConnectionType { get; set; }
        public IEnumerable<DBConnectionSetting> ConnectionStrings { get; set; }
    }
    public class DBConnectionSetting
    {
        public string Name { get; set; }

        public string ConnectionString { get; set; }
    }
}
