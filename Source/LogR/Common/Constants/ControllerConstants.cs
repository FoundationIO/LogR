using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Common.Constants
{
    public class ControllerConstants
    {
        public const string QueueAppLogUrl = "/queue/app-log";
        public const string QueueAppLogListUrl = "/queue/app-log-list";
        public const string QueuePerformanceLogUrl = "/queue/performance-log";
        public const string QueueWebLogUrl = "/queue/web-log";
        public const string QueueEventLogUrl = "/queue/event-log";
        public const string AddAppLogUrl = "/add/app-log";
        public const string AddPerformanceLogUrl = "/add/performance-log";
        public const string AddWebLogUrl = "/add/web-log";
        public const string AddEventLogUrl = "/add/event-log";
        public const string DeleteAllLogsUrl = "/log-admin/delete-all";
    }
}
