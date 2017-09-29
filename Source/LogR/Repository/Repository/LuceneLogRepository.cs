using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
//using Lucene.Net.Store;
using Framework.Infrastructure.Models.Search;
using Framework.Infrastructure.Utils;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace LogR.Repository
{
    public class LuceneLogRepository : BaseLogRepository, ILogRepository
    {
        private static bool isAppIndexExists = false;

        private static bool isPerfIndexExists = false;

        private Directory appLogDirectory;

        private Directory prefLogDirectory;

        public LuceneLogRepository(ILog log, IAppConfiguration config)
            : base(log, config)
        {
            appLogDirectory = FSDirectory.Open(config.AppLogIndexFolder);
            prefLogDirectory = FSDirectory.Open(config.PerformanceLogIndexFolder);

            if (isPerfIndexExists == false)
            {
                using (var writer = GetNewPerfWriter())
                    writer.Commit();

                isPerfIndexExists = true;
            }

            if (isAppIndexExists == false)
            {
                using (var writer = GetNewAppWriter())
                    writer.Commit();

                isAppIndexExists = true;
            }
        }

        public ReturnListModel<AppLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search)
        {
            try
            {
                using (var reader = GetNewPerfReader())
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
                    return new ReturnListModel<AppLog, PerformanceLogSearchCriteria>(search, resultList, totalRows);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Performance Log list ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<AppLog, PerformanceLogSearchCriteria>(search, "Error when getting Performance Log list ", ex);
            }
        }

        public Tuple<long, long> DeleteOldLogs(DateTime pastDate)
        {
            long appLogCount = 0, perfLogCount = 0;
            try
            {
                log.Info("Deleting Performance Log  for days less than " + pastDate);

                using (var writer = GetNewPerfWriter())
                {
                    writer.Delete<AppLog>(x => x.Longdate < pastDate);
                    writer.Commit();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting Performance Log  for days less than " + pastDate);
            }

            try
            {
                log.Info("Deleting App Log  for days less than " + pastDate);

                using (var writer = GetNewAppWriter())
                {
                    writer.Delete<AppLog>(x => x.Longdate < pastDate);
                    writer.Commit();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  for days less than " + pastDate);
            }

            var result = new Tuple<long, long>(appLogCount, perfLogCount);
            return result;
        }

        public ReturnModel<bool> DeletePerformanceLog(string id)
        {
            try
            {
                using (var writer = GetNewPerfWriter())
                {
                    writer.Delete<AppLog>(x => x.AppLogId == id);
                    writer.Commit();
                }

                return new ReturnModel<bool>(true);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting Performance Log  - id = " + id);
                return new ReturnModel<bool>(ex);
            }
        }

        public void SaveLog(RawLogData data)
        {
            if (data == null)
            {
                log.Error("Error ");
                return;
            }

            try
            {
                if (data.Type == LogType.PerformanceLog)
                {
                    SavePerformanceLogX(data.Data);
                }
                else
                {
                    SaveAppLogX(data.Data);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when processing Base Log - Message = " + data?.Data);
            }
        }

        public ReturnListModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            try
            {
                using (var reader = GetNewPerfReader())
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

                    lst = lst.OrderByDescending(x => x.Longdate);

                    var totalRows = lst.AsQueryable().Count();
                    search.TotalRowCount = totalRows;
                    var resultList = lst.ApplyPaging(search.Page, search.PageSize).ToList();
                    search.CurrentRows = resultList.Count;
                    return new ReturnListModel<AppLog, AppLogSearchCriteria>(search, resultList, totalRows);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting App Log  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<AppLog, AppLogSearchCriteria>(search, ex);
            }
        }

        public ReturnModel<bool> DeleteAppLog(string id)
        {
            try
            {
                using (var writer = GetNewAppWriter())
                {
                    writer.Delete<AppLog>(x => x.AppLogId == id);
                }

                return new ReturnModel<bool>(true);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  - id = " + id);
                return new ReturnModel<bool>(ex);
            }
        }

        public ReturnModel<DashboardSummary> GetDashboardSummary()
        {
            try
            {
                var result = new DashboardSummary();

                using (var reader = GetNewAppReader())
                {
                    var searcher = new IndexSearcher(reader);
                    result.ErrorAppLogCount = searcher.AsQueryable<AppLog>().Where(x => x.Severity == "ERROR").Count();
                    result.ErrorSqlAppLogCount = searcher.AsQueryable<AppLog>().Where(x => x.Severity == "SqlError").Count();
                    result.WarningAppLogCount = searcher.AsQueryable<AppLog>().Where(x => x.Severity == "WARN").Count();
                    result.TotalAppLogCount = searcher.AsQueryable<AppLog>().Count();
                    result.LastestAppLogs = searcher.AsQueryable<AppLog>().OrderByDescending(x => x.Longdate).Take(20).ToList();
                    result.LastestErrorAppLogs = searcher.AsQueryable<AppLog>().Where(x => x.Severity == "ERROR").OrderByDescending(x => x.Longdate).Take(20).ToList();
                }

                using (var reader = GetNewPerfReader())
                {
                    var searcher = new IndexSearcher(reader);
                    var errorLst = searcher.AsQueryable<AppLog>().Where(x => x.PerfStatus == "ERROR");
                    var allLst = searcher.AsQueryable<AppLog>();

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

        public ReturnListModel<string, BaseSearchCriteria> GetMachineNames(BaseSearchCriteria search)
        {
            try
            {
                using (var reader = GetNewAppReader())
                {
                    var searcher = new IndexSearcher(reader);
                    var lst = searcher.AsQueryable<AppLog>().Select(x => x.MachineName).ToList();
                    return new ReturnListModel<string, BaseSearchCriteria>(search, lst);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Machine Name  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<string, BaseSearchCriteria>(search, ex);
            }
        }

        public ReturnListModel<string, BaseSearchCriteria> GetAppNames(BaseSearchCriteria search)
        {
            try
            {
                using (var reader = GetNewAppReader())
                {
                    var searcher = new IndexSearcher(reader);
                    var lst = searcher.AsQueryable<AppLog>().Select(x => x.App).ToList();
                    return new ReturnListModel<string, BaseSearchCriteria>(search, lst);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting App List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<string, BaseSearchCriteria>(search, ex);
            }
        }

        public ReturnListModel<string, BaseSearchCriteria> GetSeverityNames(BaseSearchCriteria search)
        {
            try
            {
                using (var reader = GetNewAppReader())
                {
                    var searcher = new IndexSearcher(reader);
                    var lst = searcher.AsQueryable<AppLog>().Select(x => x.Severity).ToList();
                    return new ReturnListModel<string, BaseSearchCriteria>(search, lst);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Severity List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<string, BaseSearchCriteria>(search, ex);
            }
        }

        public ReturnListModel<string, BaseSearchCriteria> GetUserNames(BaseSearchCriteria search)
        {
            try
            {
                using (var reader = GetNewAppReader())
                {
                    var searcher = new IndexSearcher(reader);
                    var lst = searcher.AsQueryable<AppLog>().Select(x => x.UserIdentity).ToList();
                    return new ReturnListModel<string, BaseSearchCriteria>(search, lst);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting User name List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<string, BaseSearchCriteria>(search, ex);
            }
        }

        public Dictionary<DateTime, long> GetAppLogsStatsByDay()
        {
            Dictionary<DateTime, long> result = new Dictionary<DateTime, long>();
            try
            {
                //using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    throw new NotImplementedException();

                    //var lst = session.Query().Select(x => x.LongdateAsTicks).Distinct();
                    ////var lst = session.Query().Select(x=> new StatDate { Day = x.Longdate.Day, Month = x.Longdate.Month , Year = x.Longdate.Year } ).Distinct();
                    //var items = lst.ToList();
                    //foreach (var item in items)
                    //{
                    //    //var count = session.Query().Where(x => x.Longdate.Day == item.Day && x.Longdate.Month == item.Month && x.Longdate.Year == item.Year).LongCount();
                    //    //result.Add(new DateTime(item.Year, item.Month, item.Day), count);
                    //}
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting GetStatsByDay");
                return null;
            }

            //return result;
        }

        public void GetPerformanceLogsStatsByDay()
        {
            try
            {
                //using (var session = prefLogProvider.OpenSession<PerformanceLog>())
                {
                    throw new NotImplementedException();

                    //var lst = session.Query();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting GetPerformanceLogsStatsByDay");
            }
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

        private void SavePerformanceLogX(string message)
        {
            try
            {
                var item = GetPerformanceLogFromRawLog(message);
                using (var writer = GetNewPerfWriter())
                    writer.Add<AppLog>(item);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when saving Performance Log - Message = " + message);
            }
        }

        private void SaveAppLogX(string message)
        {
            try
            {
                var item = GetAppLogFromRawLog(message);
                using (var writer = GetNewAppWriter())
                {
                    writer.Add(item);
                    writer.Commit();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when saving App Log - Message = " + message);
            }
        }

        private ulong GetAppDataFolderSize()
        {
            return FileUtils.GetDirectorySize(config.AppLogIndexFolder);
        }

        private ulong GetPerformanceDataFolderSize()
        {
            return FileUtils.GetDirectorySize(config.PerformanceLogIndexFolder);
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

        private IndexWriter GetNewPerfWriter()
        {
            return new IndexWriter(prefLogDirectory, new IndexWriterConfig(LuceneVersion.LUCENE_48, new StandardAnalyzer(LuceneVersion.LUCENE_48)));
        }

        private IndexWriter GetNewAppWriter()
        {
            return new IndexWriter(appLogDirectory, new IndexWriterConfig(LuceneVersion.LUCENE_48, new StandardAnalyzer(LuceneVersion.LUCENE_48)));
        }

        private bool IsPerfLogIndexAvailable()
        {
            return DirectoryReader.IndexExists(prefLogDirectory);
        }

        private bool IsAppLogIndexAvailable()
        {
            return DirectoryReader.IndexExists(appLogDirectory);
        }

        private IndexReader GetNewPerfReader()
        {
            return DirectoryReader.Open(prefLogDirectory);
        }

        private IndexReader GetNewAppReader()
        {
            return DirectoryReader.Open(appLogDirectory);
        }

        private IndexReader GetNewReader(IndexWriter writer)
        {
            return DirectoryReader.Open(writer, false);
        }
    }
}
