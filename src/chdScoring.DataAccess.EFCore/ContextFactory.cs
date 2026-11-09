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
    public class ContextFactory<TContext>(IServiceProvider serviceProvider, IDatabaseConfiguration databaseConfiguration, IInterceptedDatabaseConfiguration interceptedDatabaseConfiguration) : IContextFactory<TContext>
        where TContext : DbContext
    {
        public TContext Create()
        {
            var connection = databaseConfiguration.CurrentConnection;
            if (!string.IsNullOrWhiteSpace(interceptedDatabaseConfiguration.Name)
                && databaseConfiguration.GetConnections().Any(a => a.Name == interceptedDatabaseConfiguration.Name))
            {
                connection = interceptedDatabaseConfiguration.Name;

            }

            var builder = serviceProvider.GetKeyedService<DbContextOptionsBuilder<TContext>>(connection);
            return (TContext)Activator.CreateInstance(typeof(TContext), builder.Options);
        }
    }
}
