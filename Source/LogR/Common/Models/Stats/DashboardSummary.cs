using System.Collections.Generic;
using LogR.Common.Models.Logs;

namespace LogR.Common.Models.Stats
{
    public class DashboardSummary
    {
        public long ErrorAppLogCount { get; set; }

        public long WarningAppLogCount { get; set; }

        public long TotalAppLogCount { get; set; }

        public List<AppLog> LastestAppLogs { get; set; } = new List<AppLog>();

        public List<AppLog> LastestErrorAppLogs { get; set; } = new List<AppLog>();

        public long ErrorPerformanceLogCount { get; set; }

        public long TotalPerformanceLogCount { get; set; }

        public List<AppLog> LastestPerformanceLogs { get; set; } = new List<AppLog>();

        public List<AppLog> LastestErrorPerformanceLogs { get; set; } = new List<AppLog>();

        public long ErrorSqlAppLogCount { get; set; }
    }
}
