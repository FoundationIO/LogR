using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
//using Lucene.Net.Store;
using Framework.Infrastructure.Models.Search;
using Framework.Infrastructure.Utils;
using LogR.Common.Constants;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.Log;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Linq;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace LogR.Repository.Log
{
    public class LuceneLogRepository : BaseLogRepository, ILogRepository
    {
        //private static bool isAppIndexExists = false;

        private Directory appLogDirectory;

        private LuceneDataProvider appLogProvider;

        public LuceneLogRepository(ILog log, IAppConfiguration config)
            : base(log, config)
        {
            appLogDirectory = FSDirectory.Open(config.LuceneIndexStoreSettings.AppLogIndexFolder);
            appLogProvider = new LuceneDataProvider(appLogDirectory, Lucene.Net.Util.Version.LUCENE_30);
        }

        //Save Log
        public override void SaveLog(List<RawLogData> data)
        {
            if (data == null)
            {
                log.Error("Error ");
                return;
            }

            try
            {
                var lst = GetAppLogsFromRawData(data);
                using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    session.Add(lst.ToArray());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when processing Base Log - Message");
            }
        }

        //Delete Log
        public ReturnModel<bool> DeleteAllLogs()
        {
            var errorList = new List<string>();

            var returnValue = DeleteAllLogs(StoredLogType.AppLog);
            if (returnValue.IsSuccess == false)
                errorList.Add("Unable to delete App Log");

            returnValue = DeleteAllLogs(StoredLogType.PerfLog);
            if (returnValue.IsSuccess == false)
                errorList.Add("Unable to delete App Log");

            returnValue = DeleteAllLogs(StoredLogType.EventLog);
            if (returnValue.IsSuccess == false)
                errorList.Add("Unable to delete App Log");

            returnValue = DeleteAllLogs(StoredLogType.WebLog);
            if (returnValue.IsSuccess == false)
                errorList.Add("Unable to delete App Log");

            return ReturnModel<bool>.Error(errorList);
        }

        public ReturnModel<bool> DeleteAllLogs(StoredLogType logType)
        {
            try
            {
                using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    //fixme: delete by 100 items at a time
                    var items = session.Query().Where(x => x.LogType == (int)logType).ToArray();
                    session.Delete(items);
                }

                return new ReturnModel<bool>(true);
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error when Deleting App Log  for type = {logType} ");
                return new ReturnModel<bool>(ex);
            }
        }

        public ReturnModel<bool> DeleteLog(StoredLogType logType, string id)
        {
            try
            {
                using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    //fixme: delete by 100 items at a time
                    var items = session.Query().Where(x => x.LogId == SafeUtils.Guid(id) && x.LogType == (int)logType).ToArray();
                    session.Delete(items);
                }

                return new ReturnModel<bool>(true);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  - id = " + id);
                return new ReturnModel<bool>(ex);
            }
        }

        public ReturnModel<bool> DeleteOldLogs(StoredLogType logType, DateTime pastDate)
        {
            try
            {
                log.Info("Deleting App Log  for days less than " + pastDate);
                using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    //fixme: delete by 100 items at a time
                    var items = session.Query().Where(x => x.Longdate < pastDate && x.LogType == (int)logType).ToArray();
                    session.Delete(items);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  for days less than " + pastDate);
            }

            return null;
        }

        //API for getting logs
        public ReturnListWithSearchModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            try
            {
                using (var reader = GetNewAppReader().OpenSession<AppLog>())
                {
                    LuceneQueryStatistics stats = null;

                    var lst = reader.Query();

                    lst = lst.CaptureStatistics(s => { stats = s; });

                    lst = AddFilters(lst, search.SearchTerms);

                    lst = ApplySort(lst, search.SortBy, search.SortAscending, "LongdateAsTicks");

                    var totalRows = lst.AsQueryable().Count();

                    search.TotalRowCount = totalRows;
                    var resultList = lst.ApplyPaging(search.Page, search.PageSize).ToList();
                    search.CurrentRows = resultList.Count;
                    return new ReturnListWithSearchModel<AppLog, AppLogSearchCriteria>(search, resultList, totalRows);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting App Log  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListWithSearchModel<AppLog, AppLogSearchCriteria>(search, ex);
            }
        }

        //Stats API
        public Dictionary<DateTime, long> GetAppLogsStatsByDay()
        {
            throw new NotImplementedException();
        }

        public ReturnModel<DashboardSummary> GetDashboardSummary()
        {
            try
            {
                var result = new DashboardSummary();

                using (var reader = GetNewAppReader().OpenSession<AppLog>())
                {
                    var appLog = reader.Query().Where(x => x.LogType == (int)StoredLogType.AppLog);
                    //result.ErrorAppLogCount = appLog.AsQueryable<AppLog>().Where(x => x.Severity == "ERROR").Count();
                    //result.ErrorSqlAppLogCount = appLog.AsQueryable<AppLog>().Where(x => x.Severity == "SqlError").Count();
                    //result.WarningAppLogCount = appLog.AsQueryable<AppLog>().Where(x => x.Severity == "WARN").Count();
                    //result.TotalAppLogCount = appLog.AsQueryable<AppLog>().Count();
                    //result.LastestAppLogs = appLog.AsQueryable<AppLog>().OrderByDescending(x => x.Longdate).Take(20).ToList();
                    //result.LastestErrorAppLogs = appLog.AsQueryable<AppLog>().Where(x => x.Severity == "ERROR").OrderByDescending(x => x.Longdate).Take(20).ToList();
                }

                return new ReturnModel<DashboardSummary>(result);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Dashboard Summary");
                return new ReturnModel<DashboardSummary>(ex);
            }
        }

        public void GetPerformanceLogsStatsByDay()
        {
            throw new NotImplementedException();
        }

        public ReturnModel<SystemStats> GetStats()
        {
            var stat = new SystemStats
            {
                AppDataFolderSize = GetAppDataFolderSize(),
                PerformanceDataFolderSize = GetPerformanceDataFolderSize(),
                LogFolderSize = GetLogFolderSize(),
                LogFileCount = GetLogFileCount()
            };
            return new ReturnModel<SystemStats>(stat);
        }

        protected override ReturnListWithSearchModel<string, BaseSearchCriteria> GetDistinctColumns(StoredLogType logType, BaseSearchCriteria search, Expression<Func<AppLog, string>> selector, string columnType)
        {
            try
            {
                using (var reader = GetNewAppReader().OpenSession<AppLog>())
                {
                    LuceneQueryStatistics stats = null;
                    var lst = reader.Query().Where(x => x.LogType == (int)logType).Select(selector).Distinct().CaptureStatistics(x => stats = x);
                    var totalRows = lst.AsQueryable().Count();
                    search.TotalRowCount = totalRows;
                    var resultList = lst.ApplyPaging(search.Page, search.PageSize).Distinct().ToList();
                    search.CurrentRows = resultList.Count;
                    return new ReturnListWithSearchModel<string, BaseSearchCriteria>(search, resultList, totalRows);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error when getting {columnType} list ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListWithSearchModel<string, BaseSearchCriteria>(search, $"Error when getting {columnType} list ", ex);
            }
        }

        private ulong GetAppDataFolderSize()
        {
            return FileUtils.GetDirectorySize(config.LuceneIndexStoreSettings.AppLogIndexFolder);
        }

        private ulong GetPerformanceDataFolderSize()
        {
            return FileUtils.GetDirectorySize(config.LuceneIndexStoreSettings.PerformanceLogIndexFolder);
        }

        private ulong GetLogFolderSize()
        {
            return FileUtils.GetDirectorySize(config.LogSettings.LogLocation);
        }

        private long GetLogFileCount()
        {
            var filenameList = System.IO.Directory.GetFiles(config.LogSettings.LogLocation, "*.*");
            return filenameList.LongCount();
        }

        private LuceneDataProvider GetNewAppReader()
        {
            return appLogProvider;
        }
    }
}
