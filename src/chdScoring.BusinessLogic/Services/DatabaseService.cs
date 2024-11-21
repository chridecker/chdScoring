using chdScoring.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDatabaseConfiguration _databaseConfiguration;

        public DatabaseService(IDatabaseConfiguration databaseConfiguration)
        {
            this._databaseConfiguration = databaseConfiguration;
        }

        public async Task<IEnumerable<string>> GetDatabaseConnections(CancellationToken cancellationToken = default) => this._databaseConfiguration.GetConnections().Select(s => s.Name);
        public async Task<string> GetCurrentDatabaseConnection(CancellationToken cancellationToken = default) => this._databaseConfiguration.CurrentConnection;

        public async Task<bool> SetDatabaseConnection(string name, CancellationToken cancellationToken = default)
        {
            if (this._databaseConfiguration.GetConnections().Any(s => s.Name == name) && this._databaseConfiguration.CurrentConnection != name)
            {
                this._databaseConfiguration.SetCurrentConnection(name);
                return true;
            }
            return false;
        }

    }
}
