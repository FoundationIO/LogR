using Framework.Data.Migrations;
using Framework.Infrastructure.Logging;
using LogR.Common.Interfaces.Repository;
using LogR.Repository.DbAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
