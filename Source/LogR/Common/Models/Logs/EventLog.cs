using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LogR.Common.Models.Logs
{
    public class EventLog : AppLog
    {
        protected new int LogType { get; set; }

        protected new string CorelationId { get; set; }

        protected new string FunctionId { get; set; }

        protected new int ThreadId { get; set; }

        protected new string CurrentFunction { get; set; }

        protected new string CurrentSourceFilename { get; set; }

        protected new int CurrentSourceLineNumber { get; set; }

        protected new string UserIdentity { get; set; }

        protected new string RemoteAddress { get; set; }

        protected new string UserAgent { get; set; }

        protected new string Result { get; set; }

        protected new int ResultCode { get; set; }

        protected new string PerfModule { get; set; }

        protected new string PerfFunctionName { get; set; }

        protected new DateTime StartTime { get; set; }

        protected new double ElapsedTime { get; set; }

        protected new string PerfStatus { get; set; }

        protected new string Request { get; set; }

        protected new string Response { get; set; }
    }
}
