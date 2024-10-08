using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.Repositories
{
    public interface IDurchgangRepository : IBaseEntityRepository<Round>
    {

        Task<bool> NoramlizeRound(int round, decimal normalizationCore, CancellationToken cancellationToken);
    }
}
