using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IDataImportService
    {
        Task ImportAsync(ImportRoundScoreDto dto, CancellationToken cancellationToken);
    }
}
