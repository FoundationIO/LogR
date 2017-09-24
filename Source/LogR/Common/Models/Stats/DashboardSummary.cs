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

        public List<PerformanceLog> LastestPerformanceLogs { get; set; } = new List<PerformanceLog>();

        public List<PerformanceLog> LastestErrorPerformanceLogs { get; set; } = new List<PerformanceLog>();

        public long ErrorSqlAppLogCount { get; set; }
    }
}
