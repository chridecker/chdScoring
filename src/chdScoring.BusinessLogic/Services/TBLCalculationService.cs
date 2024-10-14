using chdScoring.DataAccess.Contracts.DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace chdScoring.BusinessLogic.Services
{
    public class TBLCalculationService : ITBLCalculationService
    {
        private readonly ILogger<TBLCalculationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TBLCalculationService(ILogger<TBLCalculationService> logger, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
        }
        public async Task Calc(CancellationToken cancellationToken)
        {
            using var scope = this._serviceProvider.CreateAsyncScope();
            var dal = scope.ServiceProvider.GetRequiredService<ITBLDAL>();
            await dal.Calculate(1, cancellationToken);
        }

    }
    public interface ITBLCalculationService
    {
        Task Calc(CancellationToken cancellationToken);
    }
}
