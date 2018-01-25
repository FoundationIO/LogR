using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.Log;
using LogR.Common.Interfaces.Service;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using NetMQ;

namespace LogR.Service.Log
{
    public class LogCollectService : ILogCollectService
    {
        private NetMQQueue<RawLogData> queue;
        private NetMQPoller poller;
        private ILogRepository logRepository;

        public LogCollectService(ILogRepository logRepository, IAppConfiguration config)
        {
            this.logRepository = logRepository;
            queue = new NetMQQueue<RawLogData>();
            poller = new NetMQPoller
            {
                queue
            };

            queue.ReceiveReady += (sender, args) =>
            {
                var item = queue.Dequeue();
                var lst = new List<RawLogData>
                {
                    item
                };
                if (config.BatchSizeToIndex > 1)
                {
                    for (int i = 0; i < config.BatchSizeToIndex; ++i)
                    {
                        if (queue.TryDequeue(out RawLogData outData, new TimeSpan(10)) == false)
                        {
                            break; //no more items in the Queue so we''ll make the system wait for the Queue
                        }

                        lst.Add(outData);
                    }
                }

                Thread.Sleep(1000);
                ProcessLogFromQueue(lst);
            };
            poller.RunAsync();
        }

        public void AddToQue(StoredLogType logType, string logString, DateTime date, string applicationId)
        {
            if (string.IsNullOrEmpty(logString) == false)
                queue.Enqueue(new RawLogData { Type = logType, Data = logString , IsListData = false, ReceiveDate = date, ApplicationId = applicationId });
        }

        public void AddListToQue(StoredLogType logType, string logListString, DateTime date, string applicationId)
        {
            if (string.IsNullOrEmpty(logListString) == false)
                queue.Enqueue(new RawLogData { Type = logType, Data = logListString, IsListData = true,  ReceiveDate = date, ApplicationId = applicationId });
        }

        public void AddToDb(StoredLogType logType, string logString, DateTime date, string applicationId)
        {
            if (string.IsNullOrEmpty(logString) == false)
                logRepository.SaveLog(new RawLogData { Type = logType, Data = logString, ReceiveDate = date , ApplicationId = applicationId });
        }

        public void ProcessLogFromQueue(List<RawLogData> logDataLst)
        {
            logRepository.SaveLog(logDataLst);
        }
    }
}
