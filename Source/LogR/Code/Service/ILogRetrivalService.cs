using LogR.Code.Models;
using Framework.Models;
using LogR.Code.Models.Stats;
using LogR.Code.Models.Logs;
using LogR.Code.Models.Search;

namespace LogR.Code.Service
{
    public interface ILogRetrivalService
    {
        ReturnModel<bool> DeleteAppLog(string id);
        ReturnListModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search);
        ReturnModel<DashboardSummary> GetDashboardSummary();
        ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search);
    }
}