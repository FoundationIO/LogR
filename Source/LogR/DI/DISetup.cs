using System;
using Framework.Data.DbAccess;
using Framework.Data.Migrations;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service;
using LogR.Common.Interfaces.Service.Config;
using LogR.Repository;
using LogR.Service;
using LogR.Service.Config;
using Microsoft.Extensions.DependencyInjection;

namespace LogR.DI
{
    public static class DISetup
    {
        static DISetup()
        {
            ServiceProvider = new ServiceCollection()
                .AddApplicationDI()
                .BuildServiceProvider();
        }

        public static IServiceProvider ServiceProvider { get; set; }

        public static IServiceCollection AddApplicationDI(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging()
                .AddSingleton<AppConfiguration>()
                .AddSingleton<IBaseConfiguration, AppConfiguration>()
                .AddSingleton<IAppConfiguration, AppConfiguration>()
                .AddSingleton<ILog, Log>()
                .AddSingleton<ILogRepository, LogRepository>()
                .AddSingleton<ILogCollectService, LogCollectService>()
                .AddSingleton<ILogRetrivalService, LogRetrivalService>()
                .AddSingleton<IDBInfo, DBInfo>()
                .AddScoped<IDBManager, DBManager>()
                .AddSingleton<IDBMigration, DBMigration>()
                .AddSingleton<ISampleAppConfigFileCreator, SampleAppConfigFileCreator>();
            return serviceCollection;
        }
    }
}
