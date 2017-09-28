using Common.Interfaces.Repository;
using Framework.Data.DbAccess;
using Framework.Infrastructure.Logging;
using LogR.Common.Interfaces.Repository;
using LogR.Repository.DbAccess;

namespace Repository.DbAccess
{
    public class SqlIndexStoreDBManager : DBManager , ISqlIndexStoreDBManager
    {
        public SqlIndexStoreDBManager(ISqlIndexStoreConfiguration config, ILog log, ISqlIndexStoreDBInfo dbInfo)
            : base(config, log, dbInfo)
        {
        }
    }
}