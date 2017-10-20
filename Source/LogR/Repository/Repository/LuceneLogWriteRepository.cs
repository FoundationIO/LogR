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
using LogR.Common.Interfaces.Repository.Log;
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
    public class LuceneLogWriteRepository : BaseLogRepository, ILogWriteRepository
    {
        private static bool isAppIndexExists = false;

        private Directory appLogDirectory;

        private IndexWriter appLogWriter;

        public LuceneLogWriteRepository(ILog log, IAppConfiguration config)
            : base(log, config)
        {
            appLogDirectory = FSDirectory.Open(config.LuceneIndexStoreSettings.AppLogIndexFolder);

            appLogWriter = new IndexWriter(appLogDirectory, new IndexWriterConfig(LuceneVersion.LUCENE_48, new StandardAnalyzer(LuceneVersion.LUCENE_48)));

            if (isAppIndexExists == false)
            {
                appLogWriter.Commit();

                isAppIndexExists = true;
            }
        }

        //Save Log
        public void SaveLog(List<RawLogData> data)
        {
            if (data == null)
            {
                log.Error("Error ");
                return;
            }

            try
            {
                var lst = new List<AppLog>();
                foreach (var message in data)
                {
                    var item = this.GetLogFromRawLog<AppLog>(message.Type, message.ApplicationId, message.Data);
                    lst.Add(item);
                }

                appLogWriter.Add<AppLog>(lst);
                appLogWriter.Commit();
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when processing Base Log - Message");
            }
        }

        public void SaveLog(RawLogData data)
        {
            if (data == null)
            {
                log.Error("Error ");
                return;
            }

            SaveLog(new List<RawLogData> { data });
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
                appLogWriter.Delete<AppLog>(x => x.LogType == (int)logType);
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
                appLogWriter.Delete<AppLog>(x => x.LogId == SafeUtils.Guid(id) && x.LogType == (int)logType);
                return new ReturnModel<bool>(true);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  - id = " + id);
                return new ReturnModel<bool>(ex);
            }
        }

        public Tuple<long, long> DeleteOldLogs(StoredLogType logType, DateTime pastDate)
        {
            try
            {
                log.Info("Deleting App Log  for days less than " + pastDate);
                appLogWriter.Delete<AppLog>(x => x.Longdate < pastDate && x.LogType == (int)logType);
                appLogWriter.Commit();
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  for days less than " + pastDate);
            }

            return null;
        }
    }
}
