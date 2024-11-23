using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class PrintService : IPrintService
    {
        private readonly IPrintCache _printCache;

        public PrintService(IPrintCache printCache)
        {
            this._printCache = printCache;
        }

        public async Task<bool> PrintToPdfAsync(CreatePdfDto dto, CancellationToken cancellationToken = default)
        {
            return this._printCache.Add(dto);
        }
    }
}
