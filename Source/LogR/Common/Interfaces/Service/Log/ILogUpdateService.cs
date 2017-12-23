using Framework.Infrastructure.Models.Result;
using LogR.Common.Models.Logs;
using LogR.Common.Models.Search;
using LogR.Common.Models.Stats;

namespace LogR.Common.Interfaces.Service
{
    public interface ILogUpdateService
    {
        ReturnModel<bool> DeleteAppLog(string id);

        ReturnModel<bool> DeletePerformanceLog(string id);

        ReturnModel<bool> DeleteAllLogs(int logType);
    }
}