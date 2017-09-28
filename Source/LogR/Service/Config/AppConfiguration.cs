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
    public class AppConfiguration : BaseConfiguration , IAppConfiguration
    {
        public AppConfiguration()
        {
            PopulateFromConfigFile();
            PrepareFolders();
        }

        public int BatchSizeToIndex { get; private set; } = 1;

        public IndexStoreType IndexStoreType { get; }

        public LuceneIndexStoreSettings LuceneIndexStoreSettings { get; }

        public Sqite3IndexStoreSettings Sqite3IndexStoreSettings { get; }

        public SqlServerIndexStoreSettings SqlServerIndexStoreSettings { get; }

        public MySqlIndexStoreSettings MySqlIndexStoreSettings { get; }

        public PostgresqlIndexStoreSettings PostgresqlIndexStoreSettings { get; }

        public ElasticSearchIndexStoreSettings ElasticSearchIndexStoreSettings { get; }

        public EmbeddedElasticSearchIndexStoreSettings EmbeddedElasticSearchIndexStoreSettings { get; }

        public RaptorDBIndexStoreSettings RaptorDBIndexStoreSettings { get; }

        public MongoDBIndexStoreSettings MongoDBIndexStoreSettings { get; }

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
            BatchSizeToIndex = SafeUtils.Int(appSettings[StringConstants.Config.BatchSizeToIndex], ServerPort);
        }
    }
}