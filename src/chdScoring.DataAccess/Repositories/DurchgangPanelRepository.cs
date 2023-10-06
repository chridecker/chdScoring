using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class DurchgangPanelRepository : BaseRepository<Durchgang_Panel>, IDurchgangPanelRepository
    {
        public DurchgangPanelRepository(ILogger<DurchgangPanelRepository> logger, chdScoringContext context) : base(logger, context)
        {
        }
    }
}
