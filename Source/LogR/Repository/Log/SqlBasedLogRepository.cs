using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using Framework.Infrastructure.Utils;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.DbAccess;
using LogR.Common.Interfaces.Repository.Log;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Repository.Log
{
    public class SqlBasedLogRepository : BaseLogRepository, ILogRepository
    {
        private ISqlIndexStoreDBManager dbManager;

        public SqlBasedLogRepository(ILog log, IAppConfiguration config, ISqlIndexStoreDBManager dbManager)
            : base(log, config)
        {
            this.dbManager = dbManager;
        }

        public ReturnModel<bool> DeleteAllLogs()
        {
            dbManager.Delete<AppLog>(x => true);
            return ReturnModel<bool>.Success(true);
        }

        public ReturnModel<bool> DeleteAllLogs(StoredLogType logType)
        {
            dbManager.Delete<AppLog>(x => x.LogType == (int)logType);
            return ReturnModel<bool>.Success(true);
        }

        public ReturnModel<bool> DeleteLog(StoredLogType logType, string id)
        {
            dbManager.Delete<AppLog>(x => x.LogType == (int)logType && x.LogId == SafeUtils.Guid(id));
            return ReturnModel<bool>.Success(true);
        }

        public ReturnModel<bool> DeleteOldLogs(StoredLogType logType, DateTime pastDate)
        {
            dbManager.Delete<AppLog>(x => x.LogType == (int)logType && x.Longdate < pastDate);
            return ReturnModel<bool>.Success(true);
        }

        public override void SaveLog(List<RawLogData> data)
        {
            List<AppLog> appLogs = GetAppLogsFromRawData(data);
            dbManager.BulkCopy<AppLog>(appLogs);
        }

        public ReturnListWithSearchModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            try
            {
                var lst = dbManager.Connection.GetTable<AppLog>().Where(x => x.LogType == search.LogType).AsQueryable();

                lst = AddFilters(lst, search.SearchTerms);

                lst = ApplySort(lst, search.SortBy, search.SortAscending, "LongdateAsTicks");

                var totalRows = lst.AsQueryable().Count();

                search.TotalRowCount = totalRows;
                var resultList = lst.ApplyPaging(search.Page, search.PageSize).ToList();
                search.CurrentRows = resultList.Count;
                return new ReturnListWithSearchModel<AppLog, AppLogSearchCriteria>(search, resultList, totalRows);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Performance Log list ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListWithSearchModel<AppLog, AppLogSearchCriteria>(search, "Error when getting Performance Log list ", ex);
            }
        }

        public Dictionary<DateTime, long> GetAppLogsStatsByDay()
        {
            throw new NotImplementedException();
        }

        public ReturnModel<DashboardSummary> GetDashboardSummary()
        {
            throw new NotImplementedException();
        }

        public ReturnModel<SystemStats> GetStats()
        {
            throw new NotImplementedException();
        }

        protected override ReturnListWithSearchModel<T, BaseSearchCriteria> GetDistinctColumns<T>(StoredLogType logType, BaseSearchCriteria search, Expression<Func<AppLog, T>> selector, string columnType)
        {
            try
            {
                {
                    var lst = dbManager.Connection.GetTable<AppLog>().Where(x => x.LogType == (int)logType).AsQueryable().Select(selector);
                    var totalRows = lst.AsQueryable().Count();
                    search.TotalRowCount = totalRows;
                    var resultList = lst.ApplyPaging(search.Page, search.PageSize).Distinct().ToList();
                    search.CurrentRows = resultList.Count;
                    return new ReturnListWithSearchModel<T, BaseSearchCriteria>(search, resultList, totalRows);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, $"Error when getting {columnType} list ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListWithSearchModel<T, BaseSearchCriteria>(search, $"Error when getting {columnType} list ", ex);
            }
        }
    }
}