using Framework.Data.DbAccess;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.DbAccess;

namespace LogR.Repository.DbAccess
{
    public class SqlIndexStoreDBInfo : DBInfo , ISqlIndexStoreDBInfo
    {
        public SqlIndexStoreDBInfo(ISqlIndexStoreConfiguration config)
            : base(config)
        {
        }
    }
}
