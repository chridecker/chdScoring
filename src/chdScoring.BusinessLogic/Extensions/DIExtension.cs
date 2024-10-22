using chdScoring.BusinessLogic.Services;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Interfaces;
using chdScoring.Contracts.Settings;
using chdScoring.DataAccess.Contracts.DAL;
using chdScoring.DataAccess.Contracts.DAL.Base;
using chdScoring.DataAccess.Contracts.Interfaces;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.DAL;
using chdScoring.DataAccess.EFCore;
using chdScoring.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;



namespace chdScoring.BusinessLogic.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddchdScoringDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));

            services.Configure<DBSettings>(nameof(EDBConnection.MySql), configuration.GetSection($"{nameof(DBSettings)}:{nameof(EDBConnection.MySql)}"));
            services.Configure<DBSettings>(nameof(EDBConnection.SQLite), configuration.GetSection($"{nameof(DBSettings)}:{nameof(EDBConnection.SQLite)}"));

            services.AddSingleton<IApiLogger, ApiLogger>();
            services.AddSingleton<IFlightCacheService, FlightCacheService>();
            services.AddSingleton<DeviceStatusCache>();
            services.AddSingleton<IDeviceStatusCache>(sp => sp.GetRequiredService<DeviceStatusCache>());
            services.AddSingleton<IDeviceService>(sp => sp.GetRequiredService<DeviceStatusCache>());

            services.AddTransient<IHubDataService, HubDataService>();
            services.AddTransient<ITimerService, TimerService>();
            services.AddTransient<IJudgeService, JudgeService>();
            services.AddTransient<IScoringService, ScoringService>();
            services.AddTransient<IPilotService, PilotService>();

            services.AddContextFactory<chdScoringContext>(ServiceLifetime.Scoped);

            services.AddDALs<CurrentFlightDAL, ICurrentFlightDAL>(ServiceLifetime.Scoped);
            services.AddRepositories<TeilnehmerRepository, ITeilnehmerRepository>(ServiceLifetime.Scoped);

            return services;
        }

        private static IServiceCollection AddchdScoringContext(this IServiceCollection services)
        {
            services.AddDbContext<chdScoringContext>((sp, opt) =>
               {
                   var optMonitor = sp.GetRequiredService<IOptionsMonitor<DBSettings>>();
                   if (optMonitor.Get(nameof(EDBConnection.MySql)).ConnectionType == EDBConnection.MySql)
                   {
                       foreach (var conn in optMonitor.Get(nameof(EDBConnection.MySql)).ConnectionStrings)
                       {
                           opt.UseMySQL(conn.ConnectionString);
                       }
                   }
                   else if (optMonitor.Get(nameof(EDBConnection.SQLite)).ConnectionType == EDBConnection.SQLite)
                   {
                       foreach (var conn in optMonitor.Get(nameof(EDBConnection.SQLite)).ConnectionStrings)
                       {
                           //opt.
                           opt.UseSqlite(conn.ConnectionString);
                       }
                   }
               });
            return services;
        }
        private static IServiceCollection AddDALs<TImplementation, TInterface>(this IServiceCollection services, ServiceLifetime lifetime) where TImplementation : class, TInterface where TInterface : IBaseDAL
            => services.AddContractImplementation<IBaseDAL, TImplementation, TInterface>(lifetime);

        private static IServiceCollection AddRepositories<TImplementation, TInterface>(this IServiceCollection services, ServiceLifetime lifetime) where TImplementation : class, TInterface where TInterface : IBaseRepository
            => services.AddContractImplementation<IBaseRepository, TImplementation, TInterface>(lifetime);
        private static IServiceCollection AddContractImplementation<TContract, TImplementation, TInterface>(this IServiceCollection services, ServiceLifetime lifetime) where TImplementation : class, TInterface
        {
            var assembly = Assembly.GetAssembly(typeof(TImplementation));
            var contractAssembly = Assembly.GetAssembly(typeof(TInterface));

            foreach (var type in contractAssembly.GetTypes().Where(x => x.IsInterface && x.IsAssignableTo(typeof(TContract))))
            {
                var concreteType = assembly.GetTypes().FirstOrDefault(x => !x.IsAbstract && !x.IsInterface && x.IsAssignableTo(type));
                if (concreteType != null)
                {
                    services.Add(new(type, concreteType, lifetime));
                }
            }

            return services;
        }

        private static IServiceCollection AddContextFactory<TContext>(this IServiceCollection services, ServiceLifetime lifetime)
            where TContext : DbContext
        {
            services.AddSingleton<IDatabaseConfiguration, DatabaseConfiguration>();

            var sp = services.BuildServiceProvider();
            var optMonitor = sp.GetRequiredService<IOptionsMonitor<DBSettings>>();
            if (optMonitor.Get(nameof(EDBConnection.MySql)).ConnectionType == EDBConnection.MySql)
            {
                foreach (var conn in optMonitor.Get(nameof(EDBConnection.MySql)).ConnectionStrings)
                {
                    services.AddKeyedDBContextOptions<TContext>(conn.Name, (opt) => opt.UseMySQL(conn.ConnectionString));
                }
            }
            else if (optMonitor.Get(nameof(EDBConnection.SQLite)).ConnectionType == EDBConnection.SQLite)
            {
                foreach (var conn in optMonitor.Get(nameof(EDBConnection.SQLite)).ConnectionStrings)
                {
                    services.AddKeyedDBContextOptions<TContext>(conn.Name, (opt) => opt.UseSqlite(conn.ConnectionString));
                }
            }

            services.Add(new ServiceDescriptor(typeof(IContextFactory<TContext>), typeof(ContextFactory<TContext>), lifetime));
            return services;
        }

        private static IServiceCollection AddKeyedDBContextOptions<TContext>(this IServiceCollection services, string key, Action<DbContextOptionsBuilder<TContext>> optionsAction)
            where TContext : DbContext
        {
            var builder = new DbContextOptionsBuilder<TContext>();
            optionsAction.Invoke(builder);
            services.AddKeyedSingleton(key, builder);
            return services;
        }
    }
}
