using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Models.Config;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;

namespace LogR.Repository.DbAccess
{
    public class SqlIndexStoreConfiguration : ISqlIndexStoreConfiguration
    {
        private IAppConfiguration config;

        public SqlIndexStoreConfiguration(IAppConfiguration config)
        {
            this.config = config;

            this.LogSettings = config.LogSettings;

            if (config.IndexStoreType == Common.Enums.IndexStoreType.Sqlite3)
            {
                this.DatabaseType = DBType.SQLITE3;
                this.DatabaseName = config.Sqite3IndexStoreSettings.DbLocation;
            }
            else if (config.IndexStoreType == Common.Enums.IndexStoreType.MySql)
            {
                this.DatabaseType = DBType.MYSQL;
                this.DatabaseName = config.MySqlIndexStoreSettings.DatabaseName;
                this.DatabaseServer = config.MySqlIndexStoreSettings.DatabaseServer;
                this.DatabaseUserName = config.MySqlIndexStoreSettings.DatabaseUserName;
                this.DatabasePassword = config.MySqlIndexStoreSettings.DatabasePassword;
                this.DatabaseUseIntegratedLogin = config.MySqlIndexStoreSettings.DatabaseUseIntegratedLogin;
                this.DatabaseCommandTimeout = config.MySqlIndexStoreSettings.DatabaseCommandTimeout;
            }
            else if (config.IndexStoreType == Common.Enums.IndexStoreType.SqlServer)
            {
                this.DatabaseType = DBType.MYSQL;
                this.DatabaseName = config.SqlServerIndexStoreSettings.DatabaseName;
                this.DatabaseServer = config.SqlServerIndexStoreSettings.DatabaseServer;
                this.DatabaseUserName = config.SqlServerIndexStoreSettings.DatabaseUserName;
                this.DatabasePassword = config.SqlServerIndexStoreSettings.DatabasePassword;
                this.DatabaseUseIntegratedLogin = config.SqlServerIndexStoreSettings.DatabaseUseIntegratedLogin;
                this.DatabaseCommandTimeout = config.SqlServerIndexStoreSettings.DatabaseCommandTimeout;
            }
            else if (config.IndexStoreType == Common.Enums.IndexStoreType.Postgresql)
            {
                this.DatabaseType = DBType.POSTGRESQL;
                this.DatabaseName = config.PostgresqlIndexStoreSettings.DatabaseName;
                this.DatabaseServer = config.PostgresqlIndexStoreSettings.DatabaseServer;
                this.DatabaseUserName = config.PostgresqlIndexStoreSettings.DatabaseUserName;
                this.DatabasePassword = config.PostgresqlIndexStoreSettings.DatabasePassword;
                this.DatabaseUseIntegratedLogin = config.PostgresqlIndexStoreSettings.DatabaseUseIntegratedLogin;
                this.DatabaseCommandTimeout = config.PostgresqlIndexStoreSettings.DatabaseCommandTimeout;
            }

            this.AppName = config.AppName;

            this.MigrationNamespace = "LogR.Repository.Migration.Application";

            this.AutomaticMigration = true;

            this.MigrationProfile = "LogIndexStore";
        }

        public LogSettings LogSettings { get; private set; }

        public string DatabaseType { get; private set; }

        public string DatabaseName { get; private set; }

        public string DatabaseServer { get; private set; }

        public string DatabaseUserName { get; private set; }

        public string DatabasePassword { get; private set; }

        public bool DatabaseUseIntegratedLogin { get; private set; }

        public int DatabaseCommandTimeout { get; private set; }

        public string AppName { get; private set; }

        public string MigrationNamespace { get; private set; }

        public bool AutomaticMigration { get; private set; }

        public string MigrationProfile { get; private set; }
    }
}
