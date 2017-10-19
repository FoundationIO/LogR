using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
//using Lucene.Net.Store;
using Framework.Infrastructure.Models.Search;
using Framework.Infrastructure.Utils;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Mapper;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace LogR.Repository
{
    public class LuceneLogReadRepository : BaseLogRepository, ILogReadRepository
    {
        private Directory appLogDirectory;

        public LuceneLogReadRepository(ILog log, IAppConfiguration config)
            : base(log, config)
        {
            appLogDirectory = FSDirectory.Open(config.LuceneIndexStoreSettings.AppLogIndexFolder);
        }

        //API for getting logs
        public ReturnListWithSearchModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            try
            {
                using (var reader = GetNewAppReader())
                {
                    var searcher = new IndexSearcher(reader);
                    var lst = searcher.AsQueryable<AppLog>();
                    if (search.FromDate.IsValidDate() && search.ToDate.IsValidDate())
                    {
                        if (search.FromDate.Value <= search.ToDate.Value)
                        {
                            lst = lst.Where(x => x.Longdate >= search.FromDate.Value.StartOfDay() && x.Longdate <= search.ToDate.Value.EndOfDay());
                        }
                    }
                    else if (search.FromDate.IsValidDate())
                    {
                        lst = lst.Where(x => x.Longdate >= search.FromDate.Value.StartOfDay());
                    }
                    else if (search.ToDate.IsValidDate())
                    {
                        lst = lst.Where(x => x.Longdate <= search.ToDate.Value.EndOfDay());
                    }

                    if (search.LogType != null)
                    {
                        lst = lst.Where(x => x.Severity == search.LogType);
                    }

                    if (search.SortBy == "AppLogId")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.LogId) : lst.OrderByDescending(x => x.LogId);
                    }
                    else if (search.SortBy == "LogType")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.LogType) : lst.OrderByDescending(x => x.LogType);
                    }
                    else if (search.SortBy == "CorelationId")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.CorelationId) : lst.OrderByDescending(x => x.CorelationId);
                    }
                    else if (search.SortBy == "FunctionId")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.FunctionId) : lst.OrderByDescending(x => x.FunctionId);
                    }
                    else if (search.SortBy == "Severity")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.Severity) : lst.OrderByDescending(x => x.Severity);
                    }
                    else if (search.SortBy == "App")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.App) : lst.OrderByDescending(x => x.App);
                    }
                    else if (search.SortBy == "MachineName")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.MachineName) : lst.OrderByDescending(x => x.MachineName);
                    }
                    else if (search.SortBy == "ProcessId")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.ProcessId) : lst.OrderByDescending(x => x.ProcessId);
                    }
                    else if (search.SortBy == "ThreadId")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.ThreadId) : lst.OrderByDescending(x => x.ThreadId);
                    }
                    else if (search.SortBy == "CurrentFunction")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.CurrentFunction) : lst.OrderByDescending(x => x.CurrentFunction);
                    }
                    else if (search.SortBy == "CurrentSourceFilename")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.CurrentSourceFilename) : lst.OrderByDescending(x => x.CurrentSourceFilename);
                    }
                    else if (search.SortBy == "CurrentSourceLineNumber")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.CurrentSourceLineNumber) : lst.OrderByDescending(x => x.CurrentSourceLineNumber);
                    }
                    else if (search.SortBy == "UserIdentity")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.UserIdentity) : lst.OrderByDescending(x => x.UserIdentity);
                    }
                    else if (search.SortBy == "RemoteAddress")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.RemoteAddress) : lst.OrderByDescending(x => x.RemoteAddress);
                    }
                    else if (search.SortBy == "UserAgent")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.UserAgent) : lst.OrderByDescending(x => x.UserAgent);
                    }
                    else if (search.SortBy == "Result")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.Result) : lst.OrderByDescending(x => x.Result);
                    }
                    else if (search.SortBy == "ResultCode")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.ResultCode) : lst.OrderByDescending(x => x.ResultCode);
                    }
                    else if (search.SortBy == "Message")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.Message) : lst.OrderByDescending(x => x.Message);
                    }
                    else if (search.SortBy == "PerfModule")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.PerfModule) : lst.OrderByDescending(x => x.PerfModule);
                    }
                    else if (search.SortBy == "PerfFunctionName")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.PerfFunctionName) : lst.OrderByDescending(x => x.PerfFunctionName);
                    }
                    else if (search.SortBy == "StartTime")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.StartTime) : lst.OrderByDescending(x => x.StartTime);
                    }
                    else if (search.SortBy == "ElapsedTime")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.ElapsedTime) : lst.OrderByDescending(x => x.ElapsedTime);
                    }
                    else if (search.SortBy == "PerfStatus")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.PerfStatus) : lst.OrderByDescending(x => x.PerfStatus);
                    }
                    else if (search.SortBy == "Request")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.Request) : lst.OrderByDescending(x => x.Request);
                    }
                    else if (search.SortBy == "Response")
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.Response) : lst.OrderByDescending(x => x.Response);
                    }
                    else
                    {
                        lst = search.SortAscending ? lst.OrderBy(x => x.LongdateAsTicks) : lst.OrderByDescending(x => x.LongdateAsTicks);
                    }

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

        public ReturnListWithSearchModel<PerfLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search)
        {
            try
            {
                using (var reader = GetNewAppReader())
                {
                    var searcher = new IndexSearcher(reader);
                    var lst = searcher.AsQueryable<AppLog>();

                    if (search.FromDate.IsValidDate() && search.ToDate.IsValidDate())
                    {
                        if (search.FromDate.Value <= search.ToDate.Value)
                        {
                            lst = lst.Where(x => x.Longdate >= search.FromDate.Value.StartOfDay() && x.Longdate <= search.ToDate.Value.EndOfDay());
                        }
                    }
                    else if (search.FromDate.IsValidDate())
                    {
                        lst = lst.Where(x => x.Longdate >= search.FromDate.Value.StartOfDay());
                    }
                    else if (search.ToDate.IsValidDate())
                    {
                        lst = lst.Where(x => x.Longdate <= search.ToDate.Value.EndOfDay());
                    }

                    lst = lst.OrderByDescending(x => x.Longdate);

                    var totalRows = lst.AsQueryable().Count();
                    search.TotalRowCount = totalRows;
                    var resultList = lst.ApplyPaging(search.Page, search.PageSize).ToList();
                    search.CurrentRows = resultList.Count;
                    return new ReturnListWithSearchModel<PerfLog, PerformanceLogSearchCriteria>(search, resultList.ToPerfLogs(), totalRows);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Performance Log list ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListWithSearchModel<PerfLog, PerformanceLogSearchCriteria>(search, "Error when getting Performance Log list ", ex);
            }
        }

        public ReturnListWithSearchModel<WebLog, WebLogSearchCriteria> GetWebLogs(WebLogSearchCriteria search)
        {
            throw new NotImplementedException();
        }

        public ReturnListWithSearchModel<EventLog, EventLogSearchCriteria> GetEventLogs(EventLogSearchCriteria search)
        {
            throw new NotImplementedException();
        }

        // Parameters
        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetAppNames(StoredLogType logType, BaseSearchCriteria search)
        {
            return GetDistinctColumns(logType, search, x => x.App, "Application");
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetMachineNames(StoredLogType logType, BaseSearchCriteria search)
        {
            return GetDistinctColumns(logType, search, x => x.MachineName, "Machine");
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetUserNames(StoredLogType logType, BaseSearchCriteria search)
        {
            return GetDistinctColumns(logType, search, x => x.UserIdentity, "User");
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetSeverityNames(StoredLogType logType, BaseSearchCriteria search)
        {
            return GetDistinctColumns(logType, search, x => x.Severity, "Severity");
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

                using (var reader = GetNewAppReader())
                {
                    var searcher = new IndexSearcher(reader);
                    var appLog = searcher.AsQueryable<AppLog>().Where(x => x.LogType == (int)StoredLogType.AppLog);
                    result.ErrorAppLogCount = appLog.AsQueryable<AppLog>().Where(x => x.Severity == "ERROR").Count();
                    result.ErrorSqlAppLogCount = appLog.AsQueryable<AppLog>().Where(x => x.Severity == "SqlError").Count();
                    result.WarningAppLogCount = appLog.AsQueryable<AppLog>().Where(x => x.Severity == "WARN").Count();
                    result.TotalAppLogCount = appLog.AsQueryable<AppLog>().Count();
                    result.LastestAppLogs = appLog.AsQueryable<AppLog>().OrderByDescending(x => x.Longdate).Take(20).ToList();
                    result.LastestErrorAppLogs = appLog.AsQueryable<AppLog>().Where(x => x.Severity == "ERROR").OrderByDescending(x => x.Longdate).Take(20).ToList();

                    var perfLog = searcher.AsQueryable<AppLog>().Where(x => x.LogType == (int)StoredLogType.PerfLog);

                    var errorLst = perfLog.AsQueryable<AppLog>().Where(x => x.PerfStatus == "ERROR");
                    var allLst = perfLog.AsQueryable<AppLog>();

                    result.ErrorPerformanceLogCount = errorLst.AsQueryable().Count();
                    result.TotalPerformanceLogCount = allLst.AsQueryable().Count();

                    result.LastestPerformanceLogs = allLst.AsQueryable().OrderByDescending(x => x.Longdate).Take(20).ToList();
                    result.LastestErrorPerformanceLogs = errorLst.AsQueryable().OrderByDescending(x => x.Longdate).Take(20).ToList();
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

        protected ReturnListWithSearchModel<string, BaseSearchCriteria> GetDistinctColumns(StoredLogType logType, BaseSearchCriteria search, Expression<Func<AppLog, string>> selector, string columnType)
        {
            try
            {
                using (var reader = GetNewAppReader())
                {
                    var searcher = new IndexSearcher(reader);
                    var lst = searcher.AsQueryable<AppLog>().Select(selector).Distinct();
                    var totalRows = lst.AsQueryable().Count();
                    search.TotalRowCount = totalRows;
                    var resultList = lst.ApplyPaging(search.Page, search.PageSize).ToList();
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

        private IndexReader GetNewAppReader()
        {
            return DirectoryReader.Open(appLogDirectory);
        }
    }
}
