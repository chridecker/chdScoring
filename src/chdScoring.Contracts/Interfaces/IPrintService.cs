using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.Contracts.Interfaces
{
    public interface IPrintService
    {
        Task<bool> PrintToPdfAsync(CreatePdfDto dto, CancellationToken cancellationToken = default);
    }
}
