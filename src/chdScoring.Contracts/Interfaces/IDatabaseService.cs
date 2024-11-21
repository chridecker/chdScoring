using chdScoring.Contracts.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IDatabaseService
    {
        Task<IEnumerable<string>> GetDatabaseConnections(CancellationToken cancellationToken = default);
        Task<string> GetCurrentDatabaseConnection(CancellationToken cancellationToken = default);
        Task<bool> SetDatabaseConnection(string name, CancellationToken cancellationToken = default);
    }
}
