using System;
using System.Collections.Generic;
using Framework.Infrastructure.Models.Result;
using Framework.Infrastructure.Models.Search;
using LogR.Common.Enums;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Common.Interfaces.Repository.Log
{
    public interface ILogWriteRepository
    {
        //Save Log
        void SaveLog(List<RawLogData> data);

        void SaveLog(RawLogData data);

        //Delete Log
        ReturnModel<bool> DeleteAllLogs();

        ReturnModel<bool> DeleteAllLogs(StoredLogType logType);

        ReturnModel<bool> DeleteLog(StoredLogType logType, string id);

        Tuple<long, long> DeleteOldLogs(StoredLogType logType, DateTime pastDate);
    }
}