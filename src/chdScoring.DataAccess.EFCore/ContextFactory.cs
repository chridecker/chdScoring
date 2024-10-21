using chdScoring.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chdScoring.DataAccess.EFCore
{
    public class ContextFactory<TContext> : IContextFactory<TContext>
        where TContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDatabaseConfiguration _databaseConfiguration;

        public ContextFactory(IServiceProvider serviceProvider, IDatabaseConfiguration databaseConfiguration)
        {
            this._serviceProvider = serviceProvider;
            this._databaseConfiguration = databaseConfiguration;
        }

        public TContext Create()
        {
            var builder = this._serviceProvider.GetKeyedService<DbContextOptionsBuilder<TContext>>(this._databaseConfiguration.CurrentConnection);
            return (TContext)Activator.CreateInstance(typeof(TContext), builder.Options);
        }
    }
}
