using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using FluentNest;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using Framework.Infrastructure.Utils;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;
using Nest;

namespace LogR.Repository
{
    public class ElasticSearchLogRepository : BaseLogRepository, ILogRepository
    {
        //private static bool isAppIndexCreated = false;
        //private static bool isPerfIndexCreated = false;

        private ElasticClient client;
        private string appLogIndexName;
        private string perfLogIndexName;

        public ElasticSearchLogRepository(ILog log, IAppConfiguration config)
            : base(log, config)
        {
            var node = new Uri(config.ElasticSearchIndexStoreSettings.ServerName);
            var connectionPool = new SingleNodeConnectionPool(node);

            var settings = new ConnectionSettings(connectionPool, (Func<ConnectionSettings, IElasticsearchSerializer>)null);
            client = new ElasticClient(settings);

            appLogIndexName = (config.ElasticSearchIndexStoreSettings.AppLogIndex ?? "").ToLower();
            perfLogIndexName = (config.ElasticSearchIndexStoreSettings.PerformanceLogIndex ?? "").ToLower();

            //client.DeleteIndex(perfLogIndexName);

            if (client.IndexExists(perfLogIndexName).Exists == false)
            {
                client.CreateIndex(perfLogIndexName);
            }

            //client.DeleteIndex(appLogIndexName);

            if (client.IndexExists(appLogIndexName).Exists == false)
            {
                var mappings = new CreateIndexDescriptor(appLogIndexName).Mappings(ms => ms.Map<AppLog>(map => map.AutoMap()));
                var resp = client.CreateIndex(mappings);
            }
        }

        public ReturnListModel<AppLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search)
        {
            throw new NotImplementedException();
            /*
            try
            {
                using (var reader = GetNewPerfReader())
                {
                    var searcher = new IndexSearcher(reader);
                    var lst = searcher.AsQueryable<PerformanceLog>();

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
                    return new ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria>(search, resultList, totalRows);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Performance Log list ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria>(search, "Error when getting Performance Log list ", ex);
            }
            */
        }

        public Tuple<long, long> DeleteOldLogs(DateTime pastDate)
        {
            throw new NotImplementedException();
            /*

            long appLogCount = 0, perfLogCount = 0;
            try
            {
                log.Info("Deleting Performance Log  for days less than " + pastDate);

                using (var writer = GetNewPerfWriter())
                {
                    writer.Delete<PerformanceLog>(x => x.Longdate < pastDate);
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
            */
        }

        public ReturnModel<bool> DeletePerformanceLog(string id)
        {
            throw new NotImplementedException();
            /*
            try
            {
                using (var writer = GetNewPerfWriter())
                {
                    writer.Delete<PerformanceLog>(x => x.Id == id);
                    writer.Commit();
                }

                return new ReturnModel<bool>(true);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting Performance Log  - id = " + id);
                return new ReturnModel<bool>(ex);
            }
            */
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
                List<string> lst = new List<string>
                    {
                        data.Data
                    };

                if (data.Type == LogType.PerformanceLog)
                {
                    SavePerformanceLogX(lst);
                }
                else
                {
                    SaveAppLogX(lst);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when processing Base Log - Message = " + data?.Data);
            }
        }

        public void SaveLog(List<RawLogData> data)
        {
            if (data == null)
            {
                log.Error("Error ");
                return;
            }

            try
            {
                SaveAppLogX(data.Where(x1 => x1.Type == LogType.AppLog).Select(x2 => x2.Data).ToList());
                SavePerformanceLogX(data.Where(x1 => x1.Type == LogType.PerformanceLog).Select(x2 => x2.Data).ToList());
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when processing Base Log - Message");
            }
        }

        public ReturnListModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            try
            {
                var lst = LinqUtils.True<AppLog>();

                if (search.FromDate.IsValidDate() && search.ToDate.IsValidDate())
                {
                    if (search.FromDate.Value <= search.ToDate.Value)
                    {
                        lst = lst.Or(x => x.Longdate >= search.FromDate.Value.StartOfDay() && x.Longdate <= search.ToDate.Value.EndOfDay());
                    }
                }
                else if (search.FromDate.IsValidDate())
                {
                    lst = lst.Or(x => x.Longdate >= search.FromDate.Value.StartOfDay());
                }
                else if (search.ToDate.IsValidDate())
                {
                    lst = lst.Or(x => x.Longdate <= search.ToDate.Value.EndOfDay());
                }

                if (search.LogType != null)
                {
                    lst = lst.Or(x => x.Severity == search.LogType);
                }

                var count = client.Search<AppLog>(s =>
                    s.Index(appLogIndexName)).Total;

                var result = client.Search<AppLog>(s =>
                    s.Index(appLogIndexName).Skip(search.GetSkipValue()).Take(search.PageSize));

                var totalRows = count;
                search.TotalRowCount = totalRows;
                var resultList = result.Documents.ToList();
                search.CurrentRows = result.Documents.Count;
                return new ReturnListModel<AppLog, AppLogSearchCriteria>(search, resultList, totalRows);
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
            throw new NotImplementedException();
            /*
            try
            {
                using (var writer = GetNewAppWriter())
                {
                    writer.Delete<AppLog>(x => x.Id == id);
                }

                return new ReturnModel<bool>(true);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  - id = " + id);
                return new ReturnModel<bool>(ex);
            }
            */
        }

        public ReturnModel<DashboardSummary> GetDashboardSummary()
        {
            throw new NotImplementedException();
            /*
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
                    var errorLst = searcher.AsQueryable<PerformanceLog>().Where(x => x.Status == "ERROR");
                    var allLst = searcher.AsQueryable<PerformanceLog>();

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
            */
        }

        public ReturnListModel<string, BaseSearchCriteria> GetMachineNames(BaseSearchCriteria search)
        {
            throw new NotImplementedException();
            /*
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
            */
        }

        public ReturnListModel<string, BaseSearchCriteria> GetAppNames(BaseSearchCriteria search)
        {
            throw new NotImplementedException();
            /*
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
            */
        }

        public ReturnListModel<string, BaseSearchCriteria> GetSeverityNames(BaseSearchCriteria search)
        {
            throw new NotImplementedException();
            /*
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
            */
        }

        public ReturnListModel<string, BaseSearchCriteria> GetUserNames(BaseSearchCriteria search)
        {
            throw new NotImplementedException();
            /*
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
            */
        }

        public Dictionary<DateTime, long> GetAppLogsStatsByDay()
        {
            throw new NotImplementedException();
            /*
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
            */
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
            SystemStats stat = new SystemStats
            {
                AppDataFolderSize = GetAppDataFolderSize(),
                PerformanceDataFolderSize = GetPerformanceDataFolderSize(),
                LogFolderSize = GetLogFolderSize(),
                LogFileCount = GetLogFileCount()
            };
            return new ReturnModel<SystemStats>(stat);
        }

        public void DeleteAllAppLogs()
        {
            if (client.IndexExists(appLogIndexName).Exists)
                client.DeleteIndex(appLogIndexName);
        }

        public void DeleteAllPerformanceLogs()
        {
            if (client.IndexExists(perfLogIndexName).Exists)
                client.DeleteIndex(perfLogIndexName);
        }

        private void SavePerformanceLogX(List<string> messageList)
        {
            try
            {
                var lst = new List<AppLog>();
                foreach (var message in messageList)
                {
                    var item = GetPerformanceLogFromRawLog(message);
                    if (item == null)
                        continue;
                    lst.Add(item);
                }

                client.Bulk(x => x.CreateMany(lst).Index(perfLogIndexName));
                client.Flush(perfLogIndexName);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when saving Performance Log - Message = " + messageList.ToString(","));
            }
        }

        private void SaveAppLogX(List<string> messageList)
        {
            try
            {
                var lst = new List<AppLog>();
                foreach (var message in messageList)
                {
                    var item = GetAppLogFromRawLog(message);
                    if (item == null)
                        continue;
                    lst.Add(item);
                    client.Index<AppLog>(item, idx => idx.Index(appLogIndexName));
                }

                //client.Bulk(x => x.CreateMany(lst).Index(appLogIndexName));
                //client.Flush(appLogIndexName);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when saving App Log - Message = " + messageList.ToString(","));
            }
        }

        private ulong GetAppDataFolderSize()
        {
            return 0;
        }

        private ulong GetPerformanceDataFolderSize()
        {
            return 0;
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
    }
}
