using Framework.Infrastructure.Constants;
using LogR.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetMQ;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service;
using LogR.Common.Models.Logs;
using Framework.Infrastructure.Utils;

namespace LogR.Service
{
    public class LogCollectService : ILogCollectService
    {
        NetMQQueue<RawLogData> queue;
        NetMQPoller poller;
        ILogRepository logRepository;
        public LogCollectService(ILogRepository logRepository)
        {
            this.logRepository = logRepository;
            queue = new NetMQQueue<RawLogData>();
            poller = new NetMQPoller { queue } ;
            queue.ReceiveReady += (sender, args) => ProcessLogFromQueue(queue.Dequeue());
            poller.RunAsync();
        }
        
        public void AddToQue(LogType logType, string logString, DateTime date)
        {
            if (logString.IsTrimmedStringNullOrEmpty() == false)
                queue.Enqueue(new RawLogData { Type = logType, Data = logString , ReceiveDate = date });
        }

        public void ProcessLogFromQueue(RawLogData logData)
        {
            Task.Run(() =>
            {
                SaveToDb(logData);
            });            
        }

        public void SaveToDb(RawLogData logData)
        {
            logRepository.SaveLog(logData);
        }

    }
}
