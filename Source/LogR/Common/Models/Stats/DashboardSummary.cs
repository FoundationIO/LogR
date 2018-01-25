using System.Collections.Generic;
using LogR.Common.Models.Logs;

namespace LogR.Common.Models.Stats
{
    public class DashboardSummary
    {
        public List<KeyValuePair<int,long>> TotalLogCount { get; set; } = new List<KeyValuePair<int, long>>();

        public List<KeyValuePair<int, List<AppLog>>> LastestLogs { get; set; } = new List<KeyValuePair<int, List<AppLog>>>();

        public List<KeyValuePair<int, List<AppLog>>> LastestErrorLogs { get; set; } = new List<KeyValuePair<int, List<AppLog>>>();
    }
}
