using Framework.Infrastructure;
using LogR.Code.Infrastructure;
using LogR.Code.Repository;
using LogR.Code.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Code.DI
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
                .AddSingleton<IDBMigration, DBMigration>();            
            return serviceCollection;
        }
    }
}
