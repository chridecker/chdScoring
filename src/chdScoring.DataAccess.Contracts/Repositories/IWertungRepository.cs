using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.Repositories
{
    public interface IWertungRepository : IBaseEntityRepository<Wertung>
    {
        Task<bool> Exists(int pilot, int round, int figur, int judge, CancellationToken cancellationToken);
        Task<Wertung> Find(int pilot, int round, int figur, int judge, CancellationToken cancellationToken);
        Task<IEnumerable<Wertung>> FindByRound(int round, CancellationToken stoppingToken);
        Task<IEnumerable<Wertung>> GetScoresToPilotInRound(int id, int round, CancellationToken cancellationToken);
    }
}
