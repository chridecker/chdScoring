
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class ImageRepository : BaseRepository<Images>, IImageRepository
    {
        public ImageRepository(ILogger<ImageRepository> logger, chdScoringContext context) : base(logger, context)
        {
        }

        public Task<Images> FindById(int id, CancellationToken cancellationToken) => this._context.Images.FindAsync(id, cancellationToken).AsTask();
    }
}
