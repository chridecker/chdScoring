using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Settings;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.DAL.Base;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using chdScoring.DataAccess.DAL;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories;
using chdScoring.DataAccess.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            services.AddSingleton<IFlightCacheService, FlightCacheService>();

            services.AddTransient<ITimerService, TimerService>();
            services.AddTransient<IScoreService, ScoreService>();
            services.AddTransient<ITBLCalculationService, TBLCalculationService>();

            services.AddchdScoringContext();

            services.AddDALs<CurrentFlightDAL, ICurrentFlightDAL>();
            services.AddRepositories<TeilnehmerRepository, ITeilnehmerRepository>();

            return services;
        }

        private static IServiceCollection AddchdScoringContext(this IServiceCollection services)
        {
            services.AddDbContext<chdScoringContext>((sp, opt) =>
               {
                   var optMonitor = sp.GetRequiredService<IOptionsMonitor<DBSettings>>();
                   _ = (optMonitor.Get(nameof(EDBConnection.MySql)).ConnectionType, optMonitor.Get(nameof(EDBConnection.SQLite)).ConnectionType) switch
                   {
                       (EDBConnection.MySql, EDBConnection.None) => opt.UseMySQL(optMonitor.Get(nameof(EDBConnection.MySql)).ConnectionString),
                       (EDBConnection.None, EDBConnection.SQLite) => opt.UseSqlite(optMonitor.Get(nameof(EDBConnection.SQLite)).ConnectionString),
                   };
               });
            return services;
        }
        private static IServiceCollection AddDALs<TImplementation, TInterface>(this IServiceCollection services) where TImplementation : class, TInterface where TInterface : IBaseDAL
            => services.AddContractImplementation<IBaseDAL, TImplementation, TInterface>();

        private static IServiceCollection AddRepositories<TImplementation, TInterface>(this IServiceCollection services) where TImplementation : class, TInterface where TInterface : IBaseRepository
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
