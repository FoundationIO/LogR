using System;
using System.IO;
using System.Reflection;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Models.Config;
using Framework.Infrastructure.Utils;
using LogR.Common.Constants;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Config;
using Microsoft.Extensions.Configuration;

namespace LogR.Service.Config
{
    public class AppConfiguration : BaseConfiguration, IAppConfiguration
    {
        private IAppConfigurationFile appConfigurationFile;

        public AppConfiguration(IAppConfigurationFile appConfigurationFile)
        {
            this.appConfigurationFile = appConfigurationFile;
            PopulateFromConfigFile();
            PrepareFolders();
        }

        public int BatchSizeToIndex { get; private set; } = 1;

        public IndexStoreType IndexStoreType { get; private set; }

        public LuceneIndexStoreSettings LuceneIndexStoreSettings { get; private set; }

        public DbSettings SqlIndexStoreSettings { get; private set; }

        public ElasticSearchIndexStoreSettings ElasticSearchIndexStoreSettings { get; private set; }

        public RaptorDBIndexStoreSettings RaptorDBIndexStoreSettings { get; private set; }

        public MongoDBIndexStoreSettings MongoDBIndexStoreSettings { get; private set; }

        public int ServerPort { get; private set; }

        public bool IsSqlBasedIndexStore()
        {
            return this.IndexStoreType == IndexStoreType.MySql ||
                    this.IndexStoreType == IndexStoreType.Sqlite3 ||
                    this.IndexStoreType == IndexStoreType.SqlServer ||
                    this.IndexStoreType == IndexStoreType.Postgresql;
        }

        protected new void PrepareFolders()
        {
            base.PrepareFolders();

            if (Directory.Exists(LogSettings.LogLocation) == false)
            {
                Directory.CreateDirectory(LogSettings.LogLocation);
            }

            if (DbSettings.DatabaseType == DBType.SQLITE3)
            {
                if (Directory.Exists(FileUtils.GetFileDirectory(DbSettings.DatabaseName)) == false)
                    Directory.CreateDirectory(FileUtils.GetFileDirectory(DbSettings.DatabaseName));
            }
        }

        protected override string GetConfigFileLocation()
        {
            return appConfigurationFile.GetFileName();
        }

        protected void PopulateFromConfigFile()
        {
            var configLocation = GetConfigFileLocation();
            if (configLocation == null || configLocation.Trim() == string.Empty)
            {
                throw new Exception("Unable to find the Config location");
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(configLocation))
                .AddJsonFile(Path.GetFileName(configLocation), optional: false, reloadOnChange: true);

            var config = builder.Build();

            var appSettings = config.GetSection("configuration:appSettings");
            var frameworkLogSettings = config.GetSection("Logging");

            PopulateFromConfigFile(appSettings, frameworkLogSettings, configLocation);

            if (DbSettings.MigrationNamespace.IsTrimmedStringNullOrEmpty())
            {
                throw new Exception(ErrorConstants.MigrationNamespaceIsEmpty);
            }

            ServerPort = SafeUtils.Int(appSettings[Strings.Config.ServerPort], ServerPort);
            AppName = Path.GetFileNameWithoutExtension(this.GetType().GetTypeInfo().Assembly.Location);
            BatchSizeToIndex = SafeUtils.Int(appSettings[StringConstants.Config.BatchSizeToIndex], BatchSizeToIndex);
            this.IndexStoreType = SafeUtils.Enum<IndexStoreType>(appSettings[StringConstants.Config.IndexStoreType], IndexStoreType.None);

            if (IndexStoreType == IndexStoreType.Lucene)
            {
                var luceneSettings = appSettings.GetSection("luceneIndexStoreSettings");
                LuceneIndexStoreSettings = new LuceneIndexStoreSettings(luceneSettings, (str) =>
                {
                    if (str.IsTrimmedStringNotNullOrEmpty() && str.Contains(Strings.Config.ConfigPath))
                    {
                        str = str.Replace(Strings.Config.ConfigPath, FileUtils.GetFileDirectory(configLocation));
                        str = Path.GetFullPath(new Uri(str).LocalPath);
                    }
                    return str;
                });

                if (Directory.Exists(LuceneIndexStoreSettings.AppLogIndexFolder) == false)
                {
                    Directory.CreateDirectory(LuceneIndexStoreSettings.AppLogIndexFolder);
                }

                if (Directory.Exists(LuceneIndexStoreSettings.PerformanceLogIndexFolder) == false)
                {
                    Directory.CreateDirectory(LuceneIndexStoreSettings.PerformanceLogIndexFolder);
                }
            }
            else if (IndexStoreType == IndexStoreType.Sqlite3 || IndexStoreType == IndexStoreType.SqlServer || IndexStoreType == IndexStoreType.Postgresql || IndexStoreType == IndexStoreType.MySql)
            {
                var configSettings = appSettings.GetSection("sqlIndexStoreSettings");
                this.SqlIndexStoreSettings = new DbSettings(configSettings, (str) =>
                {
                    if (str.IsTrimmedStringNotNullOrEmpty() && str.Contains(Strings.Config.ConfigPath))
                    {
                        str = str.Replace(Strings.Config.ConfigPath, FileUtils.GetFileDirectory(configLocation));
                        str = Path.GetFullPath(new Uri(str).LocalPath);
                    }
                    return str;
                });
            }
            else if (IndexStoreType == IndexStoreType.ElasticSearch)
            {
                var configSettings = appSettings.GetSection("elasticSearchIndexStoreSettings");
                this.ElasticSearchIndexStoreSettings = new ElasticSearchIndexStoreSettings(configSettings);
            }

            //else if (IndexStoreType == IndexStoreType.EmbbededElasticSearch)
            //{
            //    var configSettings = appSettings.GetSection("embeddedElasticSearchIndexStoreSettings");
            //    this.EmbeddedElasticSearchIndexStoreSettings = new EmbeddedElasticSearchIndexStoreSettings(configSettings, (str) =>
            //    {
            //        if (str.IsTrimmedStringNotNullOrEmpty() && str.Contains(Strings.Config.ConfigPath))
            //        {
            //            str = str.Replace(Strings.Config.ConfigPath, FileUtils.GetFileDirectory(configLocation));
            //            str = Path.GetFullPath(new Uri(str).LocalPath);
            //        }
            //        return str;
            //    });
            //}
            else if (IndexStoreType == IndexStoreType.RaptorDB)
            {
                var configSettings = appSettings.GetSection("raptorDBIndexStoreSettings");
                this.RaptorDBIndexStoreSettings = new RaptorDBIndexStoreSettings(configSettings, (str) =>
                {
                    if (str.IsTrimmedStringNotNullOrEmpty() && str.Contains(Strings.Config.ConfigPath))
                    {
                        str = str.Replace(Strings.Config.ConfigPath, FileUtils.GetFileDirectory(configLocation));
                        str = Path.GetFullPath(new Uri(str).LocalPath);
                    }
                    return str;
                });
            }
            else if (IndexStoreType == IndexStoreType.MongoDB)
            {
                var configSettings = appSettings.GetSection("mongoDBIndexStoreSettings");
                this.MongoDBIndexStoreSettings = new MongoDBIndexStoreSettings(configSettings);
            }
        }
    }
}