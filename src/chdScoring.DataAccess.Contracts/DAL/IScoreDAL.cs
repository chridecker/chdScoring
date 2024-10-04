using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.DAL.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.DAL
{
    public interface IScoreDAL : IBaseDAL
    {
        Task<bool> SaveScore(SaveScoreDto dto, CancellationToken cancellationToken);
        Task<bool> UpdateScore(SaveScoreDto dto, CancellationToken cancellationToken);
    }
}
