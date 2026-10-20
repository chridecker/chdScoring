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
        private readonly IInterceptedDatabaseConfiguration _interceptedDatabaseConfiguration;

        public ContextFactory(IServiceProvider serviceProvider, IDatabaseConfiguration databaseConfiguration, IInterceptedDatabaseConfiguration interceptedDatabaseConfiguration)
        {
            this._serviceProvider = serviceProvider;
            this._databaseConfiguration = databaseConfiguration;
            _interceptedDatabaseConfiguration = interceptedDatabaseConfiguration;
        }

        public TContext Create()
        {
            var connection = this._databaseConfiguration.CurrentConnection;
            if (!string.IsNullOrWhiteSpace(_interceptedDatabaseConfiguration.Name)
                && _databaseConfiguration.GetConnections().Any(a => a.Name == _interceptedDatabaseConfiguration.Name))
            {
                connection = _interceptedDatabaseConfiguration.Name;

            }

            var builder = this._serviceProvider.GetKeyedService<DbContextOptionsBuilder<TContext>>(connection);
            return (TContext)Activator.CreateInstance(typeof(TContext), builder.Options);
        }
    }
}
