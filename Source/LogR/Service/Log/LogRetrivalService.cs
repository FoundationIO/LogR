﻿using System;
using System.Collections.Generic;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.Log;
using LogR.Common.Interfaces.Service;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Service.Log
{
    public class LogRetrivalService : ILogRetrivalService
    {
        private ILog log;
        private ILogRepository logReadRepository;

        public LogRetrivalService(ILog log, ILogRepository logReadRepository)
        {
            this.log = log;
            this.logReadRepository = logReadRepository;
        }

        public ReturnModel<DashboardSummary> GetDashboardSummary()
        {
            var result = logReadRepository.GetDashboardSummary();
            return result;
        }

        public ReturnListWithSearchModel<AppLog, AppLogSearchCriteria> GetAppLogs(AppLogSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetAppLogs(search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting App Log  List ");
                search.TotalRowCount = 0;
                search.CurrentRows = 0;
                return new ReturnListWithSearchModel<AppLog, AppLogSearchCriteria>(search, ex);
            }
        }

        public ReturnModel<SystemStats> GetStats()
        {
            try
            {
                return logReadRepository.GetStats();
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting stats");
                return new ReturnModel<SystemStats>("Error when getting stats", ex);
            }
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetAppNames(StoredLogType logType, BaseSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetAppNames(logType, search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting App Names");
                return new ReturnListModel<string>("Error when getting App Names", ex);
            }
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetMachineNames(StoredLogType logType, BaseSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetMachineNames(logType, search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Machine Names");
                return new ReturnListModel<string>("Error when getting Machine Names", ex);
            }
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetUserNames(StoredLogType logType, BaseSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetUserNames(logType, search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting User Names");
                return new ReturnListModel<string>("Error when getting User Names", ex);
            }
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetSeverityNames(StoredLogType logType, BaseSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetSeverityNames(logType, search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Log levels");
                return new ReturnListModel<string>("Error when getting Log levels", ex);
            }
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetFunctions(StoredLogType logType, BaseSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetFunctions(logType, search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Log levels");
                return new ReturnListModel<string>("Error when getting Log levels", ex);
            }
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetFiles(StoredLogType logType, BaseSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetFiles(logType, search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Log levels");
                return new ReturnListModel<string>("Error when getting Log levels", ex);
            }
        }

        public ReturnListWithSearchModel<string, BaseSearchCriteria> GetIps(StoredLogType logType, BaseSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetIps(logType, search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Log levels");
                return new ReturnListModel<string>("Error when getting Log levels", ex);
            }
        }

        public ReturnListWithSearchModel<int, BaseSearchCriteria> GetProcessIds(StoredLogType logType, BaseSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetProcessIds(logType, search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Log levels");
                return new ReturnListModel<int>("Error when getting Log levels", ex);
            }
        }

        public ReturnListWithSearchModel<int, BaseSearchCriteria> GetThreadIds(StoredLogType logType, BaseSearchCriteria search)
        {
            try
            {
                return logReadRepository.GetThreadIds(logType, search);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Log levels");
                return new ReturnListModel<int>("Error when getting Log levels", ex);
            }
        }
    }
}
