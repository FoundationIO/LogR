using System;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
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
        private ILogRepository logRepository;

        public LogRetrivalService(ILog log, ILogRepository logRepository)
        {
            this.log = log;
            this.logRepository = logRepository;
        }

        public ReturnModel<DashboardSummary> GetDashboardSummary()
        {
            var result = logRepository.GetDashboardSummary();
            return result;
        }

        public ReturnListModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            try
            {
                return logRepository.GetAppLogs(search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting App Log  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<AppLog, AppLogSearchCriteria>(search, ex);
            }
        }

        public ReturnListModel<AppLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search)
        {
            try
            {
                return logRepository.GetPerformanceLogs(search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Performance Log  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<AppLog, PerformanceLogSearchCriteria>(search, ex);
            }
        }

        public ReturnModel<bool> DeleteAppLog(string id)
        {
            try
            {
                return logRepository.DeleteAppLog(id);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  - id = " + id);
                return new ReturnModel<bool>(ex);
            }
        }

        public ReturnModel<SystemStats> GetStats()
        {
            try
            {
                return logRepository.GetStats();
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting stats");
                return new ReturnModel<SystemStats>("Error when getting stats", ex);
            }
        }
    }
}
