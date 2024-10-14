using chdScoring.Contracts.Enums;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class WettkampfLeitungRepository : BaseRepository<Wettkampf_Leitung>, IWettkampfLeitungRepository
    {
        public WettkampfLeitungRepository(ILogger<WettkampfLeitungRepository> logger, chdScoringContext context) : base(logger, context)
        {
        }

        public async Task<IEnumerable<Wettkampf_Leitung>> CurrentRoundSet(CancellationToken token)
        {
            var currentRound = await this._context.Database.SqlQueryRaw<int>($"SELECT MIN(durchgang) as Value FROM wettkampf_leitung WHERE STATUS < {(int)EFlightState.Saved}").FirstOrDefaultAsync(cancellationToken: token);
            return await this._context.Wettkampf_Leitung.Where(x => x.Durchgang == currentRound).ToListAsync(cancellationToken: token);
        }

        public async Task<Wettkampf_Leitung> GetActiveOnAirfield(int airfield, CancellationToken cancellationToken)
        => await this._context.Wettkampf_Leitung.Where(x => x.Status == (int)EFlightState.OnAir).FirstOrDefaultAsync(cancellationToken: cancellationToken);

    }
}
