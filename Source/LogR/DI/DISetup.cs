using System;
using Common.Interfaces.Repository;
using Framework.Data.DbAccess;
using Framework.Data.Migrations;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service;
using LogR.Common.Interfaces.Service.Config;
using LogR.Repository;
using LogR.Repository.DbAccess;
using LogR.Service;
using LogR.Service.Config;
using Microsoft.Extensions.DependencyInjection;
using Repository.DbAccess;

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
                .AddSingleton<IBaseConfiguration, AppConfiguration>()
                .AddSingleton<IAppConfiguration, AppConfiguration>()
                .AddSingleton<ILog, Log>()
                .AddScoped<ILogRepository, LuceneLogRepository>()
                .AddScoped<ILogCollectService, LogCollectService>()
                .AddScoped<ILogRetrivalService, LogRetrivalService>()
                .AddSingleton<IDBInfo, DBInfo>()
                .AddScoped<IDBManager, DBManager>()
                .AddSingleton<IDBMigration, DBMigration>()
                .AddScoped<ISqlIndexStoreDBManager, SqlIndexStoreDBManager>()
                .AddSingleton<ISqlIndexStoreDBInfo, SqlIndexStoreDBInfo>()
                .AddSingleton<ISampleAppConfigFileCreator, SampleAppConfigFileCreator>()
                .AddScoped<ILogRepository>(serviceProvider =>
                {
                    var config = serviceProvider.GetRequiredService<IAppConfiguration>();
                    var log = serviceProvider.GetRequiredService<ILog>();
                    var dbManager = serviceProvider.GetRequiredService<ISqlIndexStoreDBManager>();

                    var storeType = config.IndexStoreType;

                    switch (storeType)
                    {
                        case IndexStoreType.Lucene:
                            return new LuceneLogRepository(log, config);

                        case IndexStoreType.MySql:
                        case IndexStoreType.Sqlite3:
                        case IndexStoreType.Postgresql:
                        case IndexStoreType.SqlServer:
                            return new SqlBasedLogRepository(log, config, dbManager);

                        //case IndexStoreType.MongoDB:
                        //    return new MongoDBLogRepository(log, config);

                        //case IndexStoreType.RaptorDB:
                        //    return new RaptorDBLogRepository(log, config);

                        //case IndexStoreType.ElasticSearch:
                        //    return new ElasticSearchLogRepository(log, config);

                        //case IndexStoreType.EmbbededElasticSearch:
                        //    return new EmbbededElasticSearchLogRepository(log, config);

                        default:
                            throw new Exception("Index store is not configured");
                    }
                });
            return serviceCollection;
        }
    }
}