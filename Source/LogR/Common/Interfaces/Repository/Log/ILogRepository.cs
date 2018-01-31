using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using LogR.Common.Enums;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Common.Interfaces.Repository.Log
{
    public interface ILogRepository
    {
        //Save Log
        void SaveLog(List<RawLogData> data);

        void SaveLog(RawLogData data);

        //Delete Log
        ReturnModel<bool> DeleteAllLogs();

        ReturnModel<bool> DeleteAllLogs(StoredLogType logType);

        ReturnModel<bool> DeleteLog(StoredLogType logType, string id);

        ReturnModel<bool> DeleteOldLogs(StoredLogType logType, DateTime pastDate);

        //API for getting logs
        ReturnListWithSearchModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search);

        // Parameters
        ReturnListWithSearchModel<string, BaseSearchCriteria> GetAppNames(StoredLogType logType, BaseSearchCriteria search);

        ReturnListWithSearchModel<string, BaseSearchCriteria> GetMachineNames(StoredLogType logType, BaseSearchCriteria search);

        ReturnListWithSearchModel<string, BaseSearchCriteria> GetUserNames(StoredLogType logType, BaseSearchCriteria search);

        ReturnListWithSearchModel<string, BaseSearchCriteria> GetSeverityNames(StoredLogType logType, BaseSearchCriteria search);

        ReturnListWithSearchModel<string, BaseSearchCriteria> GetFunctions(StoredLogType logType, BaseSearchCriteria search);

        ReturnListWithSearchModel<string, BaseSearchCriteria> GetFiles(StoredLogType logType, BaseSearchCriteria search);

        ReturnListWithSearchModel<string, BaseSearchCriteria> GetIps(StoredLogType logType, BaseSearchCriteria search);

        ReturnListWithSearchModel<int, BaseSearchCriteria> GetProcessIds(StoredLogType logType, BaseSearchCriteria search);

        ReturnListWithSearchModel<int, BaseSearchCriteria> GetThreadIds(StoredLogType logType, BaseSearchCriteria search);

        //Stats API
        Dictionary<DateTime, long> GetAppLogsStatsByDay();

        ReturnModel<DashboardSummary> GetDashboardSummary();

        ReturnModel<SystemStats> GetStats();
    }
}