using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class PrintCache : IPrintCache
    {
        private BlockingCollection<CreatePdfDto> _toPdf = new BlockingCollection<CreatePdfDto>();
        private BlockingCollection<FileInfo> _toExecutePrint = new BlockingCollection<FileInfo>();
        private string _printer;

        private bool _autoPrint = false;
        public bool AutoPrint
        {
            get => this._autoPrint; set
            {
                this._autoPrint = value;
                this.AutoPrintChanged?.Invoke(this, value);
            }
        }
        public event EventHandler<bool> AutoPrintChanged;

        public string Printer => this._printer;
        public void SetPrinter(string printer) => this._printer = printer;
        public bool Add(CreatePdfDto dto) => this._toPdf.TryAdd(dto);
        public bool Add(FileInfo info) => this._toExecutePrint.TryAdd(info);

        public bool TryTake(out CreatePdfDto dto, CancellationToken cancellationToken = default)
        {
            dto = null;
            if (this._toPdf.TryTake(out var data, 500, cancellationToken))
            {
                dto = data;
                return true;
            }
            return false;
        }

        public bool TryTake(out FileInfo dto, CancellationToken cancellationToken = default)
        {
            dto = null;
            if (this._toExecutePrint.TryTake(out var data, 500, cancellationToken))
            {
                dto = data;
                return true;
            }
            return false;
        }

    }
}
