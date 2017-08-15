using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Lucene.Net.Store;
using Framework.Infrastructure.Models;
using Framework.Infrastructure.Utils;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;
using LogR.Common.Interfaces.Service;
using LogR.Common.Interfaces.Repository;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Config;
using LogR.Common.Interfaces;
using LogR.Common.Interfaces.Service.Config;
using Framework.Infrastructure.Constants;
using Framework.Data.DbAccess;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Framework.Infrastructure.Models.Result;

namespace LogR.Repository
{
    public class LogRepository : ILogRepository
    {
        //Directory appLogDirectory;
        //IndexWriter appLogWriter;
        //IndexReader appLogReader;
        //LuceneDataProvider appLogProvider;

        //Directory prefLogDirectory;
        //IndexWriter prefLogWriter;
        //IndexReader prefLogReader;
        //LuceneDataProvider prefLogProvider;

        ILog log;
        IAppConfiguration config;
        public LogRepository(ILog log, IAppConfiguration config)
        {
            this.log = log;
            this.config = config;

            //appLogDirectory = FSDirectory.Open(config.AppLogIndexFolder);
            //appLogWriter = new IndexWriter(appLogDirectory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED);
            //appLogReader = 

            //appLogProvider = new LuceneDataProvider(appLogDirectory, Lucene.Net.Util.Version.LUCENE_30);

            //prefLogDirectory = FSDirectory.Open(config.PerformanceLogIndexFolder);
            //prefLogWriter = new IndexWriter(prefLogDirectory, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED);
            //prefLogReader = new IndexReader();

            //prefLogProvider = new LuceneDataProvider(prefLogDirectory, Lucene.Net.Util.Version.LUCENE_30);

        }

        public ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria> GetPerformanceLogs(PerformanceLogSearchCriteria search)
        {
            try
            {
                //using (var session = prefLogProvider.OpenSession<PerformanceLog>())
                {
                    //var lst = session.Query();

                    throw new NotImplementedException();

                    //if (search.FromDate.IsValidDate() && search.ToDate.IsValidDate())
                    //{
                    //    if (search.FromDate.Value <= search.ToDate.Value)
                    //    {
                    //        lst = lst.Where(x => x.Longdate >= search.FromDate.Value.StartOfDay() && x.Longdate <= search.ToDate.Value.EndOfDay());
                    //    }
                    //}
                    //else if (search.FromDate.IsValidDate())
                    //{
                    //    lst = lst.Where(x => x.Longdate >= search.FromDate.Value.StartOfDay());
                    //}
                    //else if (search.ToDate.IsValidDate())
                    //{
                    //    lst = lst.Where(x => x.Longdate <= search.ToDate.Value.EndOfDay());
                    //}

                    //lst = lst.OrderByDescending(x => x.Longdate);

                    //var totalRows = lst.AsQueryable().Count();
                    //search.TotalRowCount = totalRows;
                    //var resultList = lst.ApplyPaging(search.Page, search.PageSize).ToList();
                    //search.CurrentRows = resultList.Count;
                    //return new ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria> { Model = resultList, Search = search, Error = ((resultList.Count == 0) ? "No Performance Logs found" : null) };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Performance Log list ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<PerformanceLog, PerformanceLogSearchCriteria>(search, null, ex);
            }
        }

        public Tuple<long, long> DeleteOldLogs(DateTime pastDate)
        {
            long appLogCount = 0, perfLogCount = 0;
            try
            {
                log.Info("Deleting Performance Log  for days less than " + pastDate);

                //using (var session = prefLogProvider.OpenSession<PerformanceLog>())
                {
                    throw new NotImplementedException();

                    //var qry = session.Query().Where(x => x.Longdate < pastDate);
                    //perfLogCount = qry.AsQueryable().LongCount();
                    //session.Delete(qry.ToArray());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting Performance Log  for days less than " + pastDate);
            }

            try
            {
                log.Info("Deleting App Log  for days less than " + pastDate);

                //using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    throw new NotImplementedException();

                    //var qry = session.Query().Where(x => x.Longdate < pastDate);
                    //appLogCount = qry.AsQueryable().LongCount();
                    //session.Delete(qry.ToArray());
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
                //using (var session = prefLogProvider.OpenSession<PerformanceLog>())
                {
                    throw new NotImplementedException();

                    //var qry = session.Query().Single(x => x.Id == id);
                    //session.Delete(qry);
                    //return new ReturnModel<bool> { Model = true };
                }
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


        private void SavePerformanceLogX(string message)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<PerformanceLog>(message);
                if (item == null)
                {
                    log.Error("Unable to deserialize the performance log message -  " + message);
                    return;
                }

                item.Id = Guid.NewGuid().ToString();
                //using (var session = prefLogProvider.OpenSession<PerformanceLog>())
                {
                    //session.Add(item);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when saving Performance Log - Message = " + message);
            }
        }


        public ReturnListModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            try
            {
                //using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    //var lst = session.Query();

                    throw new NotImplementedException();

                    //if (search.FromDate.IsValidDate() && search.ToDate.IsValidDate())
                    //{
                    //    if (search.FromDate.Value <= search.ToDate.Value)
                    //    {
                    //        lst = lst.Where(x => x.Longdate >= search.FromDate.Value.StartOfDay() && x.Longdate <= search.ToDate.Value.EndOfDay());
                    //    }
                    //}
                    //else if (search.FromDate.IsValidDate())
                    //{
                    //    lst = lst.Where(x => x.Longdate >= search.FromDate.Value.StartOfDay());
                    //}
                    //else if (search.ToDate.IsValidDate())
                    //{
                    //    lst = lst.Where(x => x.Longdate <= search.ToDate.Value.EndOfDay());
                    //}

                    //if (search.LogType != null)
                    //{
                    //    lst = lst.Where(x => x.Severity == search.LogType);
                    //}

                    //lst = lst.OrderByDescending(x => x.Longdate);

                    //var totalRows = lst.AsQueryable().Count();
                    //search.TotalRowCount = totalRows;
                    //var resultList = lst.ApplyPaging(search.Page, search.PageSize).ToList();
                    //search.CurrentRows = resultList.Count;
                    //return new ReturnListModel<AppLog, AppLogSearchCriteria> { Model = resultList, Search = search, Error = ((resultList.Count == 0) ? "No App Logs found" : null) };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting App Log  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<AppLog, AppLogSearchCriteria>(search,ex);
            }
        }


        public ReturnModel<bool> DeleteAppLog(string id)
        {
            try
            {
                //using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    throw new NotImplementedException();

                    //var qry = session.Query().Single(x => x.Id == id);
                    //session.Delete(qry);
                    //return new ReturnModel<bool> { Model = true };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  - id = " + id);
                return new ReturnModel<bool>(ex);
            }
        }

        private void SaveAppLogX(string message)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<AppLog>(message);
                if (item == null)
                {
                    log.Error("Unable to deserialize the app log message -  " + message);
                    return;
                }
                item.Id = Guid.NewGuid().ToString();
                //using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    //session.Add(item);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when saving App Log - Message = " + message);
            }
        }


        public ReturnModel<DashboardSummary> GetDashboardSummary()
        {
            try
            {
                var result = new DashboardSummary();

                //using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    //throw new NotImplementedException();

                    //var errorLst = session.Query().Where(x => x.Severity == "ERROR");
                    //var errorSqlLst = session.Query().Where(x => x.Severity == "SqlError");
                    //var warnLst = session.Query().Where(x => x.Severity == "WARN");
                    //var allLst = session.Query();

                    //result.ErrorAppLogCount = errorLst.AsQueryable().LongCount();
                    //result.ErrorSqlAppLogCount = errorSqlLst.AsQueryable().LongCount();
                    //result.WarningAppLogCount = warnLst.AsQueryable().LongCount();
                    //result.TotalAppLogCount = allLst.AsQueryable().LongCount();

                    //result.LastestAppLogs = allLst.AsQueryable().OrderByDescending(x => x.Longdate).Take(20).ToList();
                    //result.LastestErrorAppLogs = errorLst.AsQueryable().OrderByDescending(x => x.Longdate).Take(20).ToList();
                }

                //using (var session = prefLogProvider.OpenSession<PerformanceLog>())
                {
                    throw new NotImplementedException();

                    //var errorLst = session.Query().Where(x => x.Status == "ERROR");
                    //var allLst = session.Query();

                    //result.ErrorPerformanceLogCount = errorLst.AsQueryable().LongCount();
                    //result.TotalPerformanceLogCount = allLst.AsQueryable().LongCount();

                    //result.LastestPerformanceLogs = allLst.AsQueryable().OrderByDescending(x => x.Longdate).Take(20).ToList();
                    //result.LastestErrorPerformanceLogs = errorLst.AsQueryable().OrderByDescending(x => x.Longdate).Take(20).ToList();
                }

                //GetAppLogsStatsByDay();
                //GetPerformanceLogsStatsByDay();

                //return new ReturnModel<DashboardSummary> { Model = result };
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Dashboard Summary");
                return new ReturnModel<DashboardSummary>(ex);
            }
        }

        public ReturnListModel<String, BaseSearchCriteria> GetMachineNames(BaseSearchCriteria search)
        {
            try
            {
                //using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    throw new NotImplementedException();

                    //var lst = session.Query().Select(x => x.MachineName).Distinct().ToList();
                    //search.TotalRowCount = lst.Count;
                    //search.CurrentRows = lst.Count;
                    //return new ReturnListModel<string, BaseSearchCriteria> { Model = lst, Search = search, Error = null };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Machine Name  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<String, BaseSearchCriteria>(search, ex);
            }
        }

        public ReturnListModel<String, BaseSearchCriteria> GetAppNames(BaseSearchCriteria search)
        {
            try
            {
                //using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    throw new NotImplementedException();

                    //var lst = session.Query().Select(x => x.App).Distinct().ToList();
                    //search.TotalRowCount = lst.Count;
                    //search.CurrentRows = lst.Count;
                    //return new ReturnListModel<string, BaseSearchCriteria> { Model = lst, Search = search, Error = null };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting App List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<String, BaseSearchCriteria>(search, ex);
            }
        }

        public ReturnListModel<String, BaseSearchCriteria> GetSeverityNames(BaseSearchCriteria search)
        {
            try
            {
                //using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    throw new NotImplementedException();

                    //var lst = session.Query().Select(x => x.Severity).Distinct().ToList();
                    //search.TotalRowCount = lst.Count;
                    //search.CurrentRows = lst.Count;
                    //return new ReturnListModel<string, BaseSearchCriteria> { Model = lst, Search = search, Error = null };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Severity List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<String, BaseSearchCriteria>(search, ex);
            }
        }

        public ReturnListModel<String, BaseSearchCriteria> GetUserNames(BaseSearchCriteria search)
        {
            try
            {
                //using (var session = appLogProvider.OpenSession<AppLog>())
                {
                    throw new NotImplementedException();

                    //var lst = session.Query().Select(x => x.AspnetUserIdentity).Distinct().ToList();
                    //search.TotalRowCount = lst.Count;
                    //search.CurrentRows = lst.Count;
                    //return new ReturnListModel<string, BaseSearchCriteria> { Model = lst, Search = search, Error = null };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Severity List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListModel<String, BaseSearchCriteria>(search, ex);
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

        public SystemStats GetStats()
        {
            SystemStats stat = new SystemStats();
            stat.AppDataFolderSize = GetAppDataFolderSize();
            stat.PerformanceDataFolderSize = GetPerformanceDataFolderSize();
            stat.LogFolderSize = GetLogFolderSize();
            stat.LogFileCount = GetLogFileCount();
            return stat;
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

    }
}
