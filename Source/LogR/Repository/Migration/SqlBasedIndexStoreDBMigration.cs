using Framework.Data.Migrations;
using Framework.Infrastructure.Logging;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.DbAccess;
using LogR.Common.Interfaces.Repository.Log;

namespace LogR.Repository.Migration
{
    public class SqlBasedIndexStoreDBMigration : DBMigration, ISqlBasedIndexStoreDBMigration
    {
        public SqlBasedIndexStoreDBMigration(ISqlIndexStoreConfiguration config, ILog log, ISqlIndexStoreDBInfo dbInfo)
            : base(config, log, dbInfo)
        {
        }
    }
}
