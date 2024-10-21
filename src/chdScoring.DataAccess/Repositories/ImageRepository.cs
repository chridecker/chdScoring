
using chdScoring.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Repositories
{
    public class ImageRepository : BaseRepository<Images>, IImageRepository
    {
        public ImageRepository(ILogger<ImageRepository> logger,  IContextFactory<chdScoringContext> contextFactory): base(logger, contextFactory)
        {
        }

        public Task<Images> FindById(int id, CancellationToken cancellationToken) => this._context.Images.FindAsync(id, cancellationToken).AsTask();
    }
}
