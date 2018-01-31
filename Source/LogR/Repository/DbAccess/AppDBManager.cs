using Framework.Data.DbAccess;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.DbAccess;

namespace Repository.DbAccess
{
    public class AppDBManager : DBManager , IAppDBManager
    {
        public AppDBManager(IBaseConfiguration config, ILog log, IDBInfo dbInfo)
            : base(config, log, dbInfo)
        {
        }
    }
}