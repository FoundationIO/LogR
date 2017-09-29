using System;
using System.IO;
using System.Reflection;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Constants;
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
        public AppConfiguration()
        {
            PopulateFromConfigFile();
            PrepareFolders();
        }

        public int BatchSizeToIndex { get; private set; } = 1;

        public IndexStoreType IndexStoreType { get; private set; }

        public LuceneIndexStoreSettings LuceneIndexStoreSettings { get; private set; }

        public SqlBasedIndexStoreSettings Sqite3IndexStoreSettings { get; private set; }

        public SqlBasedIndexStoreSettings SqlServerIndexStoreSettings { get; private set; }

        public SqlBasedIndexStoreSettings MySqlIndexStoreSettings { get; private set; }

        public SqlBasedIndexStoreSettings PostgresqlIndexStoreSettings { get; private set; }

        public ElasticSearchIndexStoreSettings ElasticSearchIndexStoreSettings { get; private set; }

        public EmbeddedElasticSearchIndexStoreSettings EmbeddedElasticSearchIndexStoreSettings { get; private set; }

        public RaptorDBIndexStoreSettings RaptorDBIndexStoreSettings { get; private set; }

        public MongoDBIndexStoreSettings MongoDBIndexStoreSettings { get; private set; }

        public int ServerPort { get; private set; }

        public string IndexBaseFolder { get; private set; }

        public string AppLogIndexFolder
        {
            get
            {
                return IndexBaseFolder + "\\" + StringConstants.Config.AppLogIndex + "\\";
            }
        }

        public string PerformanceLogIndexFolder
        {
            get
            {
                return IndexBaseFolder + "\\" + StringConstants.Config.PerformanceLogIndex + "\\";
            }
        }

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

            if (Directory.Exists(IndexBaseFolder) == false)
            {
                Directory.CreateDirectory(IndexBaseFolder);
            }

            if (DatabaseType == DBType.SQLITE3)
            {
                if (Directory.Exists(FileUtils.GetFileDirectory(DatabaseName)) == false)
                    Directory.CreateDirectory(FileUtils.GetFileDirectory(DatabaseName));
            }
        }

        protected override string GetConfigFileLocation()
        {
            return AppConfigurationCallback.GetFileName();
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

            if (MigrationNamespace.IsTrimmedStringNullOrEmpty())
            {
                throw new Exception(ErrorConstants.MigrationNamespaceIsEmpty);
            }

            IndexBaseFolder = appSettings[StringConstants.Config.IndexBaseFolder] ?? IndexBaseFolder;
            if (IndexBaseFolder != null && IndexBaseFolder.Contains(Strings.Config.ConfigPath))
            {
                IndexBaseFolder = IndexBaseFolder.Replace(Strings.Config.ConfigPath, FileUtils.GetFileDirectory(configLocation));
                IndexBaseFolder = Path.GetFullPath(new Uri(IndexBaseFolder).LocalPath);
            }

            if (Directory.Exists(AppLogIndexFolder) == false)
            {
                Directory.CreateDirectory(AppLogIndexFolder);
            }

            if (Directory.Exists(PerformanceLogIndexFolder) == false)
            {
                Directory.CreateDirectory(PerformanceLogIndexFolder);
            }

            ServerPort = SafeUtils.Int(appSettings[Strings.Config.ServerPort], ServerPort);
            AppName = Path.GetFileNameWithoutExtension(this.GetType().GetTypeInfo().Assembly.Location);
            BatchSizeToIndex = SafeUtils.Int(appSettings[StringConstants.Config.BatchSizeToIndex], BatchSizeToIndex);

            this.IndexStoreType = SafeUtils.Enum<IndexStoreType>(appSettings[StringConstants.Config.IndexStoreType], IndexStoreType.None);

            if (IndexStoreType == IndexStoreType.Lucene)
            {
                var luceneSettings = appSettings.GetSection("luceneIndexStoreSettings");
                LuceneIndexStoreSettings = new LuceneIndexStoreSettings(luceneSettings);
            }
            else if (IndexStoreType == IndexStoreType.Sqlite3)
            {
                var sqite3IndexStoreSettings = appSettings.GetSection("sqite3IndexStoreSettings");
                this.Sqite3IndexStoreSettings = new SqlBasedIndexStoreSettings(sqite3IndexStoreSettings);
            }
            else if (IndexStoreType == IndexStoreType.SqlServer)
            {
                var configSettings = appSettings.GetSection("sqlServerIndexStoreSettings");
                this.SqlServerIndexStoreSettings = new SqlBasedIndexStoreSettings(configSettings);
            }
            else if (IndexStoreType == IndexStoreType.Postgresql)
            {
                var configSettings = appSettings.GetSection("postgresqlIndexStoreSettings");
                this.PostgresqlIndexStoreSettings = new SqlBasedIndexStoreSettings(configSettings);
            }
            else if (IndexStoreType == IndexStoreType.MySql)
            {
                var configSettings = appSettings.GetSection("mySqlIndexStoreSettings");
                this.MySqlIndexStoreSettings = new SqlBasedIndexStoreSettings(configSettings);
            }
            else if (IndexStoreType == IndexStoreType.ElasticSearch)
            {
                var configSettings = appSettings.GetSection("elasticSearchIndexStoreSettings");
                this.ElasticSearchIndexStoreSettings = new ElasticSearchIndexStoreSettings(configSettings);
            }
            else if (IndexStoreType == IndexStoreType.EmbbededElasticSearch)
            {
                var configSettings = appSettings.GetSection("embeddedElasticSearchIndexStoreSettings");
                this.EmbeddedElasticSearchIndexStoreSettings = new EmbeddedElasticSearchIndexStoreSettings(configSettings);
            }
            else if (IndexStoreType == IndexStoreType.RaptorDB)
            {
                var configSettings = appSettings.GetSection("raptorDBIndexStoreSettings");
                this.RaptorDBIndexStoreSettings = new RaptorDBIndexStoreSettings(configSettings);
            }
            else if (IndexStoreType == IndexStoreType.MongoDB)
            {
                var configSettings = appSettings.GetSection("mongoDBIndexStoreSettings");
                this.MongoDBIndexStoreSettings = new MongoDBIndexStoreSettings(configSettings);
            }
        }
    }
}