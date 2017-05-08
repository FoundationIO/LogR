using LogR.Common.Models;
using Framework.Infrastructure.Models;
using LogR.Common.Models.Stats;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;

namespace LogR.Common.Interfaces.Service
{
    public interface ILogRetrivalService
    {
        ReturnModel<bool> DeleteAppLog(string id);
        ReturnListModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search);
        ReturnModel<DashboardSummary> GetDashboardSummary();
        ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search);
    }
}