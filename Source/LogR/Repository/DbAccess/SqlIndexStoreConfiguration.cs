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

            LogSettings = config.LogSettings;

            DbSettings = config.SqlIndexStoreSettings;

            AppName = config.AppName;

            MigrationNamespace = "LogR.Repository.Migration.Application";

            AutomaticMigration = true;

            MigrationProfile = "LogIndexStore";
        }

        public string AppName { get; private set; }

        public bool AutomaticMigration { get; private set; }

        public string MigrationNamespace { get; private set; }

        public string MigrationProfile { get; private set; }

        public LogSettings LogSettings { get; private set; }

        public DbSettings DbSettings { get; private set; }
    }
}
