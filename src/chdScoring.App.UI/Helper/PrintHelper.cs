using chdScoring.App.UI.Interfaces;
using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Helper
{
    public class PrintHelper(IPrintService printService, IStammdatenService stammdatenService) : IPrintHelper
    {
        public async Task<bool> PrintRound(int pilot, int round, string? database = null, CancellationToken cancellationToken = default)
        {
            var url = await stammdatenService.GetRoundResultPdfUrl(cancellationToken);

            url = $"{url}?teilnehmer={pilot}&round={round}";
            if (!string.IsNullOrWhiteSpace(database))
            {
                url += $"&db={database.ToLower()}";
            }
            return await printService.PrintToPdfAsync(new Contracts.Dtos.CreatePdfDto()
            {
                Url = url,
                Name = $"R_{round}_P_{pilot}.pdf",
                Landscape = true,
            }, cancellationToken);
        }
    }
}
