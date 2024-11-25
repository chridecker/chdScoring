using chdScoring.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace chdScoring.Contracts.Interfaces
{
    public interface IPrintCache
    {
        string Printer { get; }
        bool AutoPrint { get; set; }

        event EventHandler<bool> AutoPrintChanged;

        bool Add(CreatePdfDto dto);
        bool Add(FileInfo info);
        void SetPrinter(string printer);
        bool TryTake(out CreatePdfDto dto, CancellationToken cancellationToken = default);
        bool TryTake(out FileInfo dto, CancellationToken cancellationToken = default);
    }
}
