using Framework.Data.DbAccess;
using LogR.Common.Interfaces.Repository;

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
