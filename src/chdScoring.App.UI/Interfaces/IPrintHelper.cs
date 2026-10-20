using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.App.UI.Interfaces
{
    public interface IPrintHelper
    {
        Task<bool> PrintRound(int pilot, int round,bool landScape = true, string? database = null, CancellationToken cancellationToken = default);
    }
}
