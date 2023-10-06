
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace chdScoring.DataAccess.Repositories
{
    public class FigurRepository : BaseRepository<Figur>, IFigurRepository
    {
        public FigurRepository(ILogger<FigurRepository> logger, chdScoringContext context) : base(logger, context)
        {
        }
    }
}
