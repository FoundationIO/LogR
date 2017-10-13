using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Common.Constants
{
    public static class StringConstants
    {
        public class Config
        {
            public const string IndexBaseFolder = "indexBaseFolder";
            public const string SqlBasedIndexStoreMigrationNamespace = "sqlBasedIndexStoreMigrationNamespace";
            public const string BatchSizeToIndex = "batchSizeToIndex";
            public const string EnablePushToMasterIndexServer = "enablePushToMasterIndexServer";
            public const string MasterIndexStoreSettings = "masterIndexStoreSettings";
            public const string IndexStoreType = "indexStoreType";
            public const string SqlIndexStoreSettings = "sqlIndexStoreSettings";
            public const string RaptorDBIndexStoreSettings = "raptorDBIndexStoreSettings";
            public const string LuceneIndexStoreSettings = "luceneIndexStoreSettings";
            public const string Sqlite3IndexStoreSettings = "sqlite3IndexStoreSettings";
            public const string DbLocation = "dbLocation";
            public const string MongoDBIndexStoreSettings = "MongoDBIndexStoreSettings";
            public const string Server = "server";
            public const string UserName = "userName";
            public const string Password = "password";
            public const string EmbbededElasticSearchIndexStoreSettings = "embbededElasticSearchIndexStoreSettings";
            public const string MySqlIndexStoreSettings = "mySqlIndexStoreSettings";
            public const string SqlServerIndexStoreSettings = "sqlServerIndexStoreSettings";
            public const string PostgresqlIndexStoreSettings = "postgresqlIndexStoreSettings";

            public const string AppLogIndex = "app_log_index";
            public const string PerformanceLogIndex = "performance_log_index";
            public const string WebLogIndex = "web_log_index";
            public const string EventLogIndex = "event_log_index";
        }

        public class SqlIndexStore
        {
            public const string MigrationNamespace = "LogR.Repository.Migration.Application";
            public const string MigrationProfile = "LogIndexStore";
        }
    }
}
