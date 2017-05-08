using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogR.Common.Interfaces.Service;
using LogR.Common.Interfaces.Repository;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Config;
using LogR.Repository;
using LogR.Service;
using LogR.Service.Config;
using LogR.Common.Interfaces.Service.Config;
using Framework.Data.DbAccess;
using Framework.Data.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace LogR.DI
{
    public static class DISetup
    {
        public static IServiceProvider ServiceProvider { get; set; }
        static DISetup()
        {
            ServiceProvider = new ServiceCollection()
                .AddApplicationDI()                
                .BuildServiceProvider();
            /*
            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");
            */
        }

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
