using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.Contracts.Dtos;

namespace chdScoring.Contracts.Interfaces
{
    public interface IStammdatenService
    {
        Task<ImageDto> GetImage(int id, CancellationToken cancellationToken);
        Task<StammdatenDto> Get(CancellationToken cancellationToken);
    }
}
