﻿using System;
using Framework.Data.DbAccess;
using Framework.Data.Migrations;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.DbAccess;
using LogR.Common.Interfaces.Repository.Log;
using LogR.Common.Interfaces.Service;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Interfaces.Service.Task;
using LogR.Repository;
using LogR.Repository.DbAccess;
using LogR.Repository.Log;
using LogR.Repository.Migration;
using LogR.Service;
using LogR.Service.Config;
using LogR.Service.Log;
using LogR.Service.Migration;
using LogR.Service.Task;
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
                .AddSingleton<ILogCollectService, LogCollectService>()
                .AddScoped<ILogRetrivalService, LogRetrivalService>()
                .AddSingleton<IDBInfo, DBInfo>()
                .AddScoped<IDBManager, DBManager>()
                .AddSingleton<IDBMigration, DBMigration>()
                .AddScoped<ISqlIndexStoreDBManager, SqlIndexStoreDBManager>()
                .AddScoped<ISqlBasedIndexStoreDBMigration, SqlBasedIndexStoreDBMigration>()
                .AddSingleton<ISqlIndexStoreDBInfo, SqlIndexStoreDBInfo>()
                .AddSingleton<ISqlIndexStoreConfiguration, SqlIndexStoreConfiguration>()
                .AddSingleton<ISampleAppConfigFileCreator, SampleAppConfigFileCreator>()
                .AddSingleton<IMigrationService, MigrationService>()
                .AddSingleton<ISeedService, SeedService>()
                .AddSingleton<ILoadTestService, LoadTestService>()
                .AddSingleton<IAppConfigurationFile, AppConfigurationFile>()
                .AddSingleton<ILogRepository>(serviceProvider =>
                {
                    var config = serviceProvider.GetRequiredService<IAppConfiguration>();
                    var log = serviceProvider.GetRequiredService<ILog>();
                    var dbManager = serviceProvider.GetRequiredService<ISqlIndexStoreDBManager>();

                    var storeType = config.IndexStoreType;

                    switch (storeType)
                    {
                        case IndexStoreType.Lucene:
                            return new LuceneLogRepository(log, config);

                        case IndexStoreType.Sqlite3:
                            return new SqlBasedLogRepository(log, config, dbManager);

                        /*
                            case IndexStoreType.MySql:
                            case IndexStoreType.Sqlite3:
                            case IndexStoreType.Postgresql:
                            case IndexStoreType.SqlServer:
                                var dbManager = serviceProvider.GetRequiredService<ISqlIndexStoreDBManager>();
                                return new SqlBasedLogRepository(log, config, dbManager);
                            //case IndexStoreType.MongoDB:
                            //    return new MongoDBLogRepository(log, config);
                            //case IndexStoreType.RaptorDB:
                            //    return new RaptorDBLogRepository(log, config);
                            case IndexStoreType.ElasticSearch:
                                return new ElasticSearchLogRepository(log, config);
                            //case IndexStoreType.EmbbededElasticSearch:
                            //    return new EmbbededElasticSearchLogRepository(log, config);
                         */
                        default:
                            throw new Exception("Index store is not configured");
                    }
                });
            return serviceCollection;
        }
    }
}