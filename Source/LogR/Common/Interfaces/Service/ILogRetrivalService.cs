using Framework.Infrastructure.Models.Result;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Common.Interfaces.Service
{
    public interface ILogRetrivalService
    {
        ReturnListWithSearchModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search);

        ReturnModel<DashboardSummary> GetDashboardSummary();

        ReturnListWithSearchModel<PerfLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search);

        ReturnModel<SystemStats> GetStats();
    }
}