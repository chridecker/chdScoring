using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.Contracts.Constants;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
using Microsoft.Extensions.Options;

namespace chdScoring.BusinessLogic.Services
{
    public class ImportService : IImportService
    {
        private readonly IOptionsMonitor<AppSettings> _optionsMonitor;

        public ImportService(IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }
        public Task<bool> ImportBinFile(ImportFileDto dto, CancellationToken cancellationToken)
        => this.CreateFile(dto, FolderConstants.Bin, cancellationToken);

        public Task<bool> ImportJsonFile(ImportFileDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ImportJsonResultFile(ImportFileDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private async Task<bool> CreateFile(ImportFileDto dto, string folder, CancellationToken cancellationToken)
        {
            var fileName = this.CreateFileName(dto);
            try
            {
                var file = Path.Combine(this._optionsMonitor.CurrentValue.ImportDirectory, folder, fileName);
                using var ms = File.Create(file);
                await ms.WriteAsync(dto.File, cancellationToken);
                ms.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string CreateFileName(ImportFileDto dto) => $"R{dto.Round}_P{dto.Round}.{dto.Type}";
    }
}
