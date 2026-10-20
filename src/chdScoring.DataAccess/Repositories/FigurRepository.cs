
using chdScoring.Contracts.Interfaces;
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
    public class FigurRepository(ILogger<FigurRepository> logger, IContextFactory<chdScoringContext> contextFactory)
        : BaseRepository<Figur>(logger, contextFactory), IFigurRepository
    {
        public async Task<IEnumerable<Figur>> GetProgramToRound(int round, CancellationToken cancellationToken)
        {
            var dg = await this._context.Set<Durchgang_Programm>().FirstOrDefaultAsync(x => x.Durchgang == round);
            var program = this._context.Set<Figur_Programm>().Where(x => x.Programm == dg.Programm);
            return await this._context.Set<Figur>().Where(x => program.Any(a => a.Figur == x.Id)).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
