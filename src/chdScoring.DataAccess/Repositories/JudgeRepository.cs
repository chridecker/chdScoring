
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
    public class JudgeRepository : BaseRepository<Judge>, IJudgeRepository
    {
        public JudgeRepository(ILogger<JudgeRepository> logger, chdScoringContext context) : base(logger, context)
        {
        }

        public async Task<IEnumerable<Judge>> GetRoundPanel(int round, CancellationToken cancellationToken)
        {
            var panel = await this._context.Durchgang_Panel.FirstOrDefaultAsync(x => x.Durchgang == round);
            var judges =  this._context.Judge_Panel.Where(x => x.Panel == panel.Panel);
            return await this._context.Judge.Where(x => judges.Any(a => a.Judge == x.Id)).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
