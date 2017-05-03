using System;
using Framework.Contants;
using LogR.Code.Models;
using LogR.Code.Models.Logs;

namespace LogR.Code.Service
{
    public interface ILogCollectService
    {
        void AddToQue(LogType logType, string logString, DateTime date);
        void ProcessLogFromQueue(RawLogData logData);
        void SaveToDb(RawLogData logData);
    }
}