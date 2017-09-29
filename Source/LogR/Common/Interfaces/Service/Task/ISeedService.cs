using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogR.Common.Models.Logs;

namespace LogR.Common.Interfaces.Service.Task
{
    public interface ISeedService
    {
        List<AppLog> GetAppLogs(int numberOfLogs);

        void GenerateLogs(int numberOfLogs);
    }
}
