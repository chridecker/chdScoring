
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.Repositories
{
    public interface ITeilnehmerDurchgangJudgeRespository : IBaseEntityRepository<Teilnehmer_Durchgang_Judge>
    {
        Task<bool> Exists(int pilot, int round,  int judge, CancellationToken cancellationToken);
    }
}
