using Framework.Infrastructure;
using Framework.Models;
using LogR.Code.Infrastructure;
using LogR.Code.Models.Logs;
using LogR.Code.Models.Search;
using LogR.Code.Models.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogR.Code.Service
{
    public class LogRetrivalService : ILogRetrivalService
    {
        ILog log;
        public LogRetrivalService(ILog log)
        {
            this.log = log;
        }

        public ReturnModel<DashboardSummary> GetDashboardSummary()
        {
            var result = new ReturnModel<DashboardSummary>() { Model = new DashboardSummary() } ;
            return result;
        }

        public ReturnListModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            try
            {
                var resultList = new List<AppLog>();
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<AppLog, AppLogSearchCriteria> { Model = resultList, Search = search, Error = ((resultList.Count == 0) ? "No App Logs found" : null) };
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting App Log  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<AppLog, AppLogSearchCriteria> { Model = null, Search = search, Error = ex.Message };
            }
        }

        public ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search)
        {
            try
            {
                var resultList = new List<PerformanceLog>();
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria> { Model = resultList, Search = search, Error = ((resultList.Count == 0) ? "No Performance Logs found" : null) };
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Performance Log  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria> { Model = null, Search = search, Error = ex.Message };
            }
        }


        public ReturnModel<bool> DeleteAppLog(string id)
        {
            try
            {
                return new ReturnModel<bool> { Model = true };
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  - id = " + id);
                return new ReturnModel<bool> { Model = false, Error = ex.Message };
            }
        }

    }
}
