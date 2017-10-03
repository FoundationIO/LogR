using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Common.Interfaces.Repository
{
    public interface ILogRepository
    {
        void DeleteAllAppLogs();

        void DeleteAllPerformanceLogs();

        ReturnModel<bool> DeleteAppLog(string id);

        Tuple<long, long> DeleteOldLogs(DateTime pastDate);

        ReturnModel<bool> DeletePerformanceLog(string id);

        ReturnListModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search);

        Dictionary<DateTime, long> GetAppLogsStatsByDay();

        ReturnListModel<string, BaseSearchCriteria> GetAppNames(BaseSearchCriteria search);

        ReturnModel<DashboardSummary> GetDashboardSummary();

        ReturnListModel<string, BaseSearchCriteria> GetMachineNames(BaseSearchCriteria search);

        ReturnListModel<AppLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search);

        void GetPerformanceLogsStatsByDay();

        ReturnListModel<string, BaseSearchCriteria> GetSeverityNames(BaseSearchCriteria search);

        ReturnModel<SystemStats> GetStats();

        ReturnListModel<string, BaseSearchCriteria> GetUserNames(BaseSearchCriteria search);

        void SaveLog(List<RawLogData> data);

        void SaveLog(RawLogData data);
    }
}