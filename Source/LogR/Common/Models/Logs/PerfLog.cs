using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LogR.Common.Models.Logs
{
    public class PerfLog : AppLog
    {
        protected new int LogType { get; set; }

        protected new string Severity { get; set; }

        protected new int ProcessId { get; set; }

        protected new int ThreadId { get; set; }

        protected new string CurrentFunction { get; set; }

        protected new string CurrentSourceFilename { get; set; }

        protected new int CurrentSourceLineNumber { get; set; }

        protected new string Result { get; set; }

        protected new string Message { get; set; }
    }
}
