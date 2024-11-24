using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Helper
{
    public class PrintHelper : IPrintHelper
    {
        private readonly IPrintService _printService;

        public PrintHelper(IPrintService printService)
        {
            this._printService = printService;
        }
        public async Task<bool> PrintRound(int pilot, int round, string? database = null, CancellationToken cancellationToken = default)
        {
            var url = $"http://localhost/print_durchgang.php?teilnehmer={pilot}&round={round}";
            if (!string.IsNullOrWhiteSpace(database))
            {
                url += $"&db={database.ToLower()}";
            }
            return await this._printService.PrintToPdfAsync(new Contracts.Dtos.CreatePdfDto()
            {
                Url = url,
                Name = $"R_{round}_P_{pilot}.pdf",
                Landscape = true,
            }, cancellationToken);
        }
    }
}
