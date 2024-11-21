using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        private string _currentConnectionKey;
        private readonly IOptionsMonitor<DBSettings> _optionsMonitor;


        public event EventHandler<string> ConnectionChanged;

        public string CurrentConnection => this._currentConnectionKey;

        public DatabaseConfiguration(IOptionsMonitor<DBSettings> optionsMonitor)
        {
            this._optionsMonitor = optionsMonitor;
            this._currentConnectionKey = this.GetConnections().FirstOrDefault()?.Name;
        }

        public void SetCurrentConnection(string connectionKey)
        {
            this._currentConnectionKey = connectionKey;
            this.ConnectionChanged?.Invoke(this, connectionKey);
        }

        public IEnumerable<DBConnectionSetting> GetConnections()
        {
            if (this._optionsMonitor.Get(nameof(EDBConnection.MySql)).ConnectionType == EDBConnection.MySql)
            {
                return this._optionsMonitor.Get(nameof(EDBConnection.MySql)).ConnectionStrings;
            }
            else if (this._optionsMonitor.Get(nameof(EDBConnection.SQLite)).ConnectionType == EDBConnection.SQLite)
            {
                return this._optionsMonitor.Get(nameof(EDBConnection.SQLite)).ConnectionStrings;
            }
            return Enumerable.Empty<DBConnectionSetting>();
        }
    }
}
