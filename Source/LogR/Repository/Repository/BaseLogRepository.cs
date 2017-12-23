using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using Framework.Infrastructure.Utils;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Repository
{
    public class BaseLogRepository
    {
        protected ILog log;

        protected IAppConfiguration config;

        public BaseLogRepository(ILog log, IAppConfiguration config)
        {
            this.log = log;
            this.config = config;
        }

        protected T GetLogFromRawLog<T>(StoredLogType logType, string applicationId, string message)
            where T : AppLog
        {
            var outerData = JsonUtils.Deserialize<RawLogData>(message);
            if (outerData == null)
            {
                throw new Exception("Unable to deserialize the log message -  " + message);
            }

            var item = JsonUtils.Deserialize<T>(outerData.Data);

            if (item == null)
            {
                throw new Exception("Unable to deserialize the log internal message -  " + outerData.Data);
            }

            item.LogId = Guid.NewGuid();
            item.LogType = (int)logType;
            item.ApplicationId = applicationId;
            if (item.Longdate.IsInvalidDate())
                item.Longdate = DateTime.UtcNow;
            item.ReceivedDate = outerData.ReceiveDate;
            item.LongdateAsTicks = item.Longdate.Ticks;
            return item;
        }
    }
}