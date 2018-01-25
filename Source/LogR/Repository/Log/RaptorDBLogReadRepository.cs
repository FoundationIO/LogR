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
using LogR.Common.Interfaces.Repository.Log;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Repository
{
    public class RaptorDBLogReadRepository : BaseLogRepository, ILogReadRepository
    {
        private ISqlIndexStoreDBManager dbManager;

        public RaptorDBLogReadRepository(ILog log, IAppConfiguration config, ISqlIndexStoreDBManager dbManager)
            : base(log, config)
        {
            this.dbManager = dbManager;
        }

        public ReturnListWithSearchModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            throw new NotImplementedException();
        }

        public Dictionary<DateTime, long> GetAppLogsStatsByDay()
        {
            throw new NotImplementedException();
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetAppNames(StoredLogType logType, BaseSearchCriteria search)
        {
            throw new NotImplementedException();
        }

        public ReturnModel<DashboardSummary> GetDashboardSummary()
        {
            throw new NotImplementedException();
        }

        public ReturnListWithSearchModel<EventLog, EventLogSearchCriteria> GetEventLogs(EventLogSearchCriteria search)
        {
            throw new NotImplementedException();
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetMachineNames(StoredLogType logType, BaseSearchCriteria search)
        {
            throw new NotImplementedException();
        }

        public ReturnListWithSearchModel<PerfLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search)
        {
            throw new NotImplementedException();
        }

        public void GetPerformanceLogsStatsByDay()
        {
            throw new NotImplementedException();
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetSeverityNames(StoredLogType logType, BaseSearchCriteria search)
        {
            throw new NotImplementedException();
        }

        public ReturnModel<SystemStats> GetStats()
        {
            throw new NotImplementedException();
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetUserNames(StoredLogType logType, BaseSearchCriteria search)
        {
            throw new NotImplementedException();
        }

        public ReturnListWithSearchModel<WebLog, WebLogSearchCriteria> GetWebLogs(WebLogSearchCriteria search)
        {
            throw new NotImplementedException();
        }
    }
}