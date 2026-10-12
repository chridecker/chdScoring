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
        Task<bool> ImportJsonResultFile(ImportFileDto dto, CancellationToken cancellationToken);
    }
}
