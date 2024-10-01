using chdScoring.PrintService.Dtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.PrintService.Services
{
    public class PrintCache : IPrintCache
    {
        private BlockingCollection<PrintDto> _toPrint = new BlockingCollection<PrintDto>();
        private string _printer;

        public string Printer => this._printer;
        public void SetPrinter(string printer) => this._printer = printer;
        public bool Add(PrintDto dto) => this._toPrint.TryAdd(dto);
        public bool TryTake(out PrintDto dto, CancellationToken cancellationToken = default)
        {
            dto = null;
            if (this._toPrint.TryTake(out var data, 500, cancellationToken))
            {
                dto = data;
                return true;
            }
            return false;
        }

    }
    public interface IPrintCache
    {
        string Printer {get;}
        bool Add(PrintDto dto);
        void SetPrinter(string printer);
        bool TryTake(out PrintDto dto, CancellationToken cancellationToken = default);
    }
}
