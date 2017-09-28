using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using Framework.Infrastructure.Utils;
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

        protected AppLog GetAppLogFromRawLog(string message)
        {
            var item = JsonUtils.Deserialize<AppLog>(message);
            if (item == null)
            {
                throw new Exception("Unable to deserialize the app log message -  " + message);
            }

            item.Id = Guid.NewGuid();
            item.LogType = (int)LogType.AppLog;
            if (item.Longdate == null)
                item.Longdate = DateTime.UtcNow;
            item.LongdateAsTicks = item.Longdate.Value.Ticks;
            return item;
        }

        protected AppLog GetPerformanceLogFromRawLog(string message)
        {
            var item = JsonUtils.Deserialize<AppLog>(message);
            if (item == null)
            {
                throw new Exception("Unable to deserialize the performance log message -  " + message);
            }

            item.Id = Guid.NewGuid();
            item.LogType = (int)LogType.PerformanceLog;
            if (item.Longdate == null)
                item.Longdate = DateTime.UtcNow;
            item.LongdateAsTicks = item.Longdate.Value.Ticks;
            return item;
        }
    }
}