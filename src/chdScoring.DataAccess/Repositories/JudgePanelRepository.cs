
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace chdScoring.DataAccess.Repositories
{
    public class JudgePanelRepository : BaseRepository<Judge_Panel>, IJudgePanelRepository
    {
        public JudgePanelRepository(ILogger<JudgePanelRepository> logger, chdScoringContext context) : base(logger, context)
        {
        }
    }
}
