using System;
using System.Collections.Generic;
using Elasticsearch.Net;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository.Log;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using Nest;

namespace LogR.Repository
{
    public class ElasticSearchLogWriteRepository : BaseLogRepository, ILogWriteRepository
    {
        //private static bool isAppIndexCreated = false;
        //private static bool isPerfIndexCreated = false;

        private ElasticClient client;
        private string appLogIndexName;
        private string perfLogIndexName;

        public ElasticSearchLogWriteRepository(ILog log, IAppConfiguration config)
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

        public ReturnModel<bool> DeleteAllLogs()
        {
            if (client.IndexExists(appLogIndexName).Exists)
                client.DeleteIndex(appLogIndexName);

            if (client.IndexExists(perfLogIndexName).Exists)
                client.DeleteIndex(perfLogIndexName);

            return ReturnModel<bool>.Success(true);
        }

        public ReturnModel<bool> DeleteAllLogs(StoredLogType logType)
        {
            if (logType == StoredLogType.AppLog)
            {
                if (client.IndexExists(appLogIndexName).Exists)
                    client.DeleteIndex(appLogIndexName);
            }

            if (logType == StoredLogType.PerfLog)
            {
                if (client.IndexExists(perfLogIndexName).Exists)
                    client.DeleteIndex(perfLogIndexName);
            }

            return ReturnModel<bool>.Success(true);
        }

        public ReturnModel<bool> DeleteLog(StoredLogType logType, string id)
        {
            throw new NotImplementedException();
        }

        public Tuple<long, long> DeleteOldLogs(StoredLogType logType, DateTime pastDate)
        {
            throw new NotImplementedException();
        }

        public void SaveLog(List<RawLogData> data)
        {
            throw new NotImplementedException();
        }

        public void SaveLog(RawLogData data)
        {
            throw new NotImplementedException();
        }

        /*
        public ReturnModel<bool> DeleteOldLogs(StoredLogType storedLogType, DateTime pastDate)

        {
            throw new NotImplementedException();

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
        }

        public ReturnModel<bool> DeleteLog(StoredLogType storedLogType, string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllLogs()
        {
            if (client.IndexExists(appLogIndexName).Exists)
                client.DeleteIndex(appLogIndexName);

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
    */
    }
}
