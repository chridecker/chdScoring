﻿
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
    public class CountryImageRepository : BaseRepository<Country_Images>, ICountryImageRepository
    {
        public CountryImageRepository(ILogger<CountryImageRepository> logger,  IContextFactory<chdScoringContext> contextFactory) : base(logger, contextFactory)
        {
        }
        public Task<Country_Images> FindById(int id, CancellationToken cancellationToken) => this._context.Country_Images.FindAsync(id, cancellationToken).AsTask();
    }
}