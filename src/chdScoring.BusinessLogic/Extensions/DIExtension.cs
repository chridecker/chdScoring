using chdScoring.Contracts.Settings;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace chdScoring.BusinessLogic.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddchdScoringDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DBSettings>(configuration.GetSection(nameof(DBSettings)));

            services.AddDbContext<chdScoringContext>((sp, opt) =>
            {
                opt.UseSqlite(sp.GetRequiredService<IOptionsMonitor<DBSettings>>().CurrentValue.ConnectionString);
            });


            services.AddRepositories<TeilnehmerRepository, ITeilnehmerRepository>();

            return services;
        }
        public static IServiceCollection AddRepositories<TImplementation, TInterface>(this IServiceCollection services) where TImplementation : class, TInterface where TInterface : IBaseRepository
            => services.AddContractImplementation<IBaseRepository,TImplementation, TInterface>();
        private static IServiceCollection AddContractImplementation<TContract,TImplementation, TInterface>(this IServiceCollection services) where TImplementation : class, TInterface
        {
            var assembly = Assembly.GetAssembly(typeof(TImplementation));
            var contractAssembly = Assembly.GetAssembly(typeof(TInterface));

            foreach (var type in contractAssembly.GetTypes().Where(x => x.IsInterface && x.IsAssignableTo(typeof(TContract))))
            {
                var concreteType = assembly.GetTypes().FirstOrDefault(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableTo(type));
                if (concreteType != null)
                {
                    services.AddTransient(type, concreteType);
                }
            }

            return services;
        }
    }
}
