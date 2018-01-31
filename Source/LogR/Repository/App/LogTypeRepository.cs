using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Framework.Data.DbAccess;
using LogR.Common.Interfaces.Repository.App;
using LogR.Common.Interfaces.Repository.DbAccess;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.DB;

namespace LogR.Repository.App
{
    public class LogTypeRepository : ILogTypeRepository
    {
        private ILog log;
        private IAppConfiguration config;
        private IAppDBManager dbManager;

        public LogTypeRepository(ILog log, IAppConfiguration config, IAppDBManager dbManager)
        {
            this.log = log;
            this.config = config;
            this.dbManager = dbManager;
        }

        public List<LogTypeConfig> GetLogTypeConfig()
        {
            return dbManager.SelectAll<LogTypeConfig>();
        }
    }
}
