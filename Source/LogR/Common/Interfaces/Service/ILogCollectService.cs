using System;
using Framework.Infrastructure.Constants;
using LogR.Common.Models;
using LogR.Common.Models.Logs;

namespace LogR.Common.Interfaces.Service
{
    public interface ILogCollectService
    {
        void AddToQue(LogType logType, string logString, DateTime date);
        void ProcessLogFromQueue(RawLogData logData);
        void SaveToDb(RawLogData logData);
    }
}