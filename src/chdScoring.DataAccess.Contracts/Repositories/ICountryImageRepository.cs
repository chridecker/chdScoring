using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.Contracts.Repositories
{
    public interface ICountryImageRepository : IBaseEntityRepository<Country_Images>
    {
        Task<Country_Images> FindById(int id, CancellationToken cancellationToken);
    }
}
