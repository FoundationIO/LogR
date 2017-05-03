using System;
using System.Collections.Generic;
using Framework.Models;
using LogR.Code.Models.Search;
using LogR.Code.Models.Logs;
using LogR.Code.Models.Stats;

namespace LogR.Code.Repository
{
    public interface ILogRepository
    {
        ReturnModel<bool> DeleteAppLog(string id);
        Tuple<long, long> DeleteOldLogs(DateTime pastDate);
        ReturnModel<bool> DeletePerformanceLog(string id);
        ReturnListModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search);
        Dictionary<DateTime, long> GetAppLogsStatsByDay();
        ReturnListModel<string, BaseSearchCriteria> GetAppNames(BaseSearchCriteria search);
        ReturnModel<DashboardSummary> GetDashboardSummary();
        ReturnListModel<string, BaseSearchCriteria> GetMachineNames(BaseSearchCriteria search);
        ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search);
        void GetPerformanceLogsStatsByDay();
        ReturnListModel<string, BaseSearchCriteria> GetSeverityNames(BaseSearchCriteria search);
        SystemStats GetStats();
        ReturnListModel<string, BaseSearchCriteria> GetUserNames(BaseSearchCriteria search);
        void SaveLog(RawLogData data);
    }
}