using chdScoring.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Settings
{
    public class DBSettings
    {
        public EDBConnection ConnectionType { get; set; }
        public string ConnectionString { get; set; }
    }
}
