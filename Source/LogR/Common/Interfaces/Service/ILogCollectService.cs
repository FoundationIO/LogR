using System;
using Framework.Infrastructure.Constants;

namespace LogR.Common.Interfaces.Service
{
    public interface ILogCollectService
    {
        void AddToQue(LogType logType, string logString, DateTime date);

        void AddToDb(LogType logType, string logString, DateTime date);
    }
}