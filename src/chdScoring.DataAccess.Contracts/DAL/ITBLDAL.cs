using chdScoring.DataAccess.Contracts.DAL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.DAL
{
    public interface ITBLDAL : IBaseDAL
    {
        Task Calculate(int round, CancellationToken stoppingToken);
    }
}
