using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using Framework.Infrastructure.Utils;
using LinqToDB;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Repository
{
    public class RaptorDBLogWriteRepository : BaseLogRepository, ILogWriteRepository
    {
        private ISqlIndexStoreDBManager dbManager;

        public RaptorDBLogWriteRepository(ILog log, IAppConfiguration config, ISqlIndexStoreDBManager dbManager)
            : base(log, config)
        {
            this.dbManager = dbManager;
        }

        public ReturnModel<bool> DeleteAllLogs()
        {
            throw new NotImplementedException();
        }

        public ReturnModel<bool> DeleteAllLogs(StoredLogType logType)
        {
            throw new NotImplementedException();
        }

        public ReturnModel<bool> DeleteLog(StoredLogType logType, string id)
        {
            throw new NotImplementedException();
        }

        public Tuple<long, long> DeleteOldLogs(StoredLogType logType, DateTime pastDate)
        {
            throw new NotImplementedException();
        }

        public void SaveLog(List<RawLogData> data, int applicationId)
        {
            throw new NotImplementedException();
        }

        public void SaveLog(RawLogData data, int applicationId)
        {
            throw new NotImplementedException();
        }
   }
}