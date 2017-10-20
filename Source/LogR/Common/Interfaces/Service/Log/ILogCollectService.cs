using System;
using Framework.Infrastructure.Constants;
using LogR.Common.Enums;

namespace LogR.Common.Interfaces.Service
{
    public interface ILogCollectService
    {
        void AddToQue(StoredLogType logType, string logString, DateTime date, int applicationId);

        void AddToDb(StoredLogType logType, string logString, DateTime date, int applicationId);
    }
}