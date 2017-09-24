using System;
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service;
using LogR.Common.Models.Logs;
using NetMQ;

namespace LogR.Service
{
    public class LogCollectService : ILogCollectService
    {
        private NetMQQueue<RawLogData> queue;
        private NetMQPoller poller;
        private ILogRepository logRepository;

        public LogCollectService(ILogRepository logRepository)
        {
            this.logRepository = logRepository;
            queue = new NetMQQueue<RawLogData>();
            poller = new NetMQPoller
            {
                queue
            };
            queue.ReceiveReady += (sender, args) => ProcessLogFromQueue(queue.Dequeue());
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

        public void ProcessLogFromQueue(RawLogData logData)
        {
            //fixme: Optimize this
            Task.Run(() =>
            {
                SaveToDb(logData);
            });
        }

        private void SaveToDb(RawLogData logData)
        {
            logRepository.SaveLog(logData);
        }
    }
}
