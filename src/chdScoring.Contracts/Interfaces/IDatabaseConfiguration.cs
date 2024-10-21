using chdScoring.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.Contracts.Interfaces
{
    public interface IDatabaseConfiguration
    {
        public string CurrentConnection { get; }
        public void SetCurrentConnection(string connectionKey);
        public IEnumerable<DBConnectionSetting> GetConnections();
    }
}
