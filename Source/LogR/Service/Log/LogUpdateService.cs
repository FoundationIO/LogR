using System;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Models.Result;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.Log;
using LogR.Common.Interfaces.Service;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;

namespace LogR.Service.Log
{
    public class LogUpdateService : ILogUpdateService
    {
        private ILog log;
        private ILogWriteRepository logWriteRepository;

        public LogUpdateService(ILog log, ILogWriteRepository logWriteRepository)
        {
            this.log = log;
            this.logWriteRepository = logWriteRepository;
        }

        public ReturnModel<bool> DeleteAppLog(string id)
        {
            try
            {
                return logWriteRepository.DeleteLog(StoredLogType.AppLog, id);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting App Log  - id = " + id);
                return new ReturnModel<bool>(ex);
            }
        }

        public ReturnModel<bool> DeletePerformanceLog(string id)
        {
            try
            {
                return logWriteRepository.DeleteLog(StoredLogType.PerfLog, id);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error when getting Deleting Performance Log  - id = " + id);
                return new ReturnModel<bool>(ex);
            }
        }
    }
}
