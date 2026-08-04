using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.Contracts.Dtos;

namespace chdScoring.Contracts.Interfaces
{
    public interface IImportService
    {
        Task<bool> ImportBinFile(ImportFileDto dto, CancellationToken cancellationToken);
        Task<bool> ImportJsonFile(ImportFileDto dto, CancellationToken cancellationToken);
        Task<bool> ImportJsonResultFile(ImportFileDto dto, CancellationToken cancellationToken);
    }
}
