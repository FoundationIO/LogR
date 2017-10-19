using System;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Service
{
    public class LogRetrivalService : ILogRetrivalService
    {
        private ILog log;
        private ILogReadRepository logReadRepository;

        public LogRetrivalService(ILog log, ILogReadRepository logReadRepository)
        {
            this.log = log;
            this.logReadRepository = logReadRepository;
        }

        public ReturnModel<DashboardSummary> GetDashboardSummary()
        {
            var result = logReadRepository.GetDashboardSummary();
            return result;
        }

        public ReturnListWithSearchModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetAppLogs(search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting App Log  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListWithSearchModel<AppLog, AppLogSearchCriteria>(search, ex);
            }
        }

        public ReturnListWithSearchModel<PerfLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetPerformanceLogs(search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Performance Log  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListWithSearchModel<PerfLog, PerformanceLogSearchCriteria>(search, ex);
            }
        }

        public ReturnModel<SystemStats> GetStats()
        {
            try
            {
                return logReadRepository.GetStats();
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting stats");
                return new ReturnModel<SystemStats>("Error when getting stats", ex);
            }
        }
    }
}
