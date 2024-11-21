using chdScoring.PrintService.Dtos;
using System.Collections.Concurrent;
using System.IO;

namespace chdScoring.PrintService.Services
{
    public class PrintCache : IPrintCache
    {
        private BlockingCollection<CreatePdfDto> _toPdf = new BlockingCollection<CreatePdfDto>();
        private BlockingCollection<PrintDto> _toPrint = new BlockingCollection<PrintDto>();
        private BlockingCollection<PrintDto> _toExecutePrint = new BlockingCollection<PrintDto>();
        private string _printer;

        public string Printer => this._printer;
        public void SetPrinter(string printer) => this._printer = printer;
        public bool Add(CreatePdfDto dto) => this._toPdf.TryAdd(dto);
        public bool Add(PrintDto dto) => this._toPrint.TryAdd(dto);
        public bool Add(FileInfo info, PrintDto dto)
        {
            dto.FilePath = info.FullName;
            return this._toExecutePrint.TryAdd(dto);
        }
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

        public bool TryTake(FileInfo fileInfo, out PrintDto dto, CancellationToken cancellationToken = default)
        {
            dto = null;
            var lst = this._toPrint.TakeWhile(x => this.IsPrintDto(x, fileInfo));
            if (lst.Any())
            {
                dto = lst.FirstOrDefault();
                return true;
            }
            return false;
        }
        private bool IsPrintDto(PrintDto dto, FileInfo info) => dto.FilePath == info.Name;

        public bool TryTake(out PrintDto dto, CancellationToken cancellationToken = default)
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
    public interface IPrintCache
    {
        string Printer { get; }
        bool Add(CreatePdfDto dto);
        bool Add(PrintDto dto);
        bool Add(FileInfo info, PrintDto dto);
        void SetPrinter(string printer);
        bool TryTake(out CreatePdfDto dto, CancellationToken cancellationToken = default);
        bool TryTake(FileInfo info, out PrintDto dto, CancellationToken cancellationToken = default);
        bool TryTake(out PrintDto dto, CancellationToken cancellationToken = default);
    }
}
