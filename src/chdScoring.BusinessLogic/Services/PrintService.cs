using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<bool> ChangeAutoPrint(CancellationToken cancellationToken = default)
        {
            this._printCache.AutoPrint = !this._printCache.AutoPrint;
            return this._printCache.AutoPrint;
        }

        public Task<bool> GetAutoPrintSetting(CancellationToken cancellationToken = default) => Task.FromResult(this._printCache.AutoPrint);

        public async Task<IEnumerable<PrintPdfDto>> GetPdfLst(CancellationToken cancellationToken = default)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), FolderConstants.Folder);
            if (Directory.Exists(dir) && Directory.EnumerateFiles(dir, "*.pdf").Any())
            {
                return Directory.EnumerateFiles(dir, "*.pdf").Select(s => new PrintPdfDto()
                {
                    Directory = s,
                    Name = new FileInfo(s).Name,
                    CreationTime = new FileInfo(s).CreationTime
                });
            }
            return Enumerable.Empty<PrintPdfDto>();
        }

        public async Task<bool> AddToPrintCache(PrintPdfDto dto, CancellationToken cancellationToken = default)
        {
            var info = new FileInfo(dto.Directory);
            if (info.Exists)
            {
                return this._printCache.Add(info);
            }
            return false;
        }

        public Task<bool> PrintToPdfAsync(CreatePdfDto dto, CancellationToken cancellationToken = default) => Task.FromResult(this._printCache.Add(dto));
    }
}