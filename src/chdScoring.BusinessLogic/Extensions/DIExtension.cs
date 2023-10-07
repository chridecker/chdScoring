using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Enums;
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
using System.Diagnostics;
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
            services.Configure<DBSettings>(nameof(EDBConnection.MySql), configuration.GetSection($"{nameof(DBSettings)}:{nameof(EDBConnection.MySql)}"));
            services.Configure<DBSettings>(nameof(EDBConnection.SQLite), configuration.GetSection($"{nameof(DBSettings)}:{nameof(EDBConnection.SQLite)}"));

            services.AddSingleton<IApiLogger, ApiLogger>();
            services.AddSingleton<ILogNotifyService, LogNotifyService>();

            services.AddDbContext<chdScoringContext>((sp, opt) =>
            {
                var setting = (sp.GetRequiredService<IOptionsMonitor<DBSettings>>()) switch
                {
                    var s when s.Get(nameof(EDBConnection.MySql)).ConnectionType != EDBConnection.None => s.Get(nameof(EDBConnection.MySql)),
                    var s when s.Get(nameof(EDBConnection.SQLite)).ConnectionType != EDBConnection.None => s.Get(nameof(EDBConnection.SQLite)),
                };
                switch (setting.ConnectionType)
                {
                    case EDBConnection.MySql:
                        opt.UseMySQL(setting.ConnectionString);
                        break;
                    case EDBConnection.SQLite:
                        opt.UseSqlite(setting.ConnectionString);
                        break;
                }
            });


            services.AddRepositories<TeilnehmerRepository, ITeilnehmerRepository>();

            return services;
        }
        public static IServiceCollection AddRepositories<TImplementation, TInterface>(this IServiceCollection services) where TImplementation : class, TInterface where TInterface : IBaseRepository
            => services.AddContractImplementation<IBaseRepository, TImplementation, TInterface>();
        private static IServiceCollection AddContractImplementation<TContract, TImplementation, TInterface>(this IServiceCollection services) where TImplementation : class, TInterface
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
