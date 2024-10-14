using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class KlasseRepository : BaseRepository<Klassen>, IKlasseRepository
    {
        public KlasseRepository(ILogger<KlasseRepository> logger, chdScoringContext context) : base(logger, context)
        {
        }
        public async Task<Klassen> GetCurrentKlasse(CancellationToken cancellationToken)
        {
            var st = await this._context.Stammdaten.FirstOrDefaultAsync(cancellationToken: cancellationToken);  
            return await this._context.Klassen.FirstOrDefaultAsync(x => x.Id == st.Klasse, cancellationToken);
        }
    }
}
