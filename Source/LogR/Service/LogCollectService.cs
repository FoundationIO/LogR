using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Models.Logs;
using NetMQ;

namespace LogR.Service
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
                var lst = new List<RawLogData>();
                lst.Add(item);
                if (config.BatchSizeToIndex > 1)
                {
                    for (int i = 0; i < config.BatchSizeToIndex; ++i)
                    {
                        RawLogData outData;
                        if (queue.TryDequeue(out outData, new TimeSpan(10)) == false)
                        {
                            break; //no more items in the Queue so we''ll make the system wait for the Queue
                        }

                        lst.Add(outData);
                    }
                }

                ProcessLogFromQueue(lst);
            };
            poller.RunAsync();
        }

        public void AddToQue(LogType logType, string logString, DateTime date)
        {
            if (string.IsNullOrEmpty(logString) == false)
                queue.Enqueue(new RawLogData { Type = logType, Data = logString , ReceiveDate = date });
        }

        public void AddToDb(LogType logType, string logString, DateTime date)
        {
            if (string.IsNullOrEmpty(logString) == false)
                logRepository.SaveLog(new RawLogData { Type = logType, Data = logString, ReceiveDate = date });
        }

        public void ProcessLogFromQueue(List<RawLogData> logDataLst)
        {
            logRepository.SaveLog(logDataLst);
        }
    }
}
