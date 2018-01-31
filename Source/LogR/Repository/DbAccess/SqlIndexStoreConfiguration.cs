using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Models.Config;
using LogR.Common.Constants;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.DbAccess;
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

            if (DbSettings != null)
            {
                DbSettings.MigrationNamespace = StringConstants.SqlIndexStore.MigrationNamespace;

                DbSettings.AutomaticMigration = true;

                DbSettings.MigrationProfile = StringConstants.SqlIndexStore.MigrationProfile;
            }
        }

        public string AppName { get; private set; }

        public LogSettings LogSettings { get; private set; }

        public DbSettings DbSettings { get; private set; }
    }
}
