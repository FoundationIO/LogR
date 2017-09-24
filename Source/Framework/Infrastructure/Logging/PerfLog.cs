using System;
using System.Collections.Generic;

namespace Framework.Infrastructure.Logging
{
    public class PerfLog : IDisposable
    {
        private readonly string module = string.Empty;
        private readonly string function = string.Empty;
        private bool started = false;
        private bool autoCloseIsError = true;
        private bool logToDefaultLogger = false;
        private ILog log;
        private DateTime startTime, endTime;

        public PerfLog(ILog log, string moduleName, string functionName, bool startMeasuringOnCreate, bool autoCloseIsError, bool logToDefaultLogger = true)
        {
            module = moduleName;
            function = functionName;
            this.autoCloseIsError = autoCloseIsError;
            this.logToDefaultLogger = logToDefaultLogger;
            this.log = log;
            if (startMeasuringOnCreate)
            {
                Start();
            }
        }

        public void Start()
        {
            if (started != true)
            {
                startTime = DateTime.Now;
                started = true;
            }
        }

        public void StopAndWriteCompleteLog(string additionalMsg = "")
        {
            StopAndWriteToLog("Completed", additionalMsg);
        }

        public void StopAndWriteErrorLog(string additionalMsg = "")
        {
            StopAndWriteToLog("Error", additionalMsg);
        }

        void IDisposable.Dispose()
        {
            if (started == true)
            {
                if (autoCloseIsError)
                    StopAndWriteErrorLog();
                else
                    StopAndWriteCompleteLog();
            }
        }

        private void StopAndWriteToLog(string status = "completed", string additionalMsg = "")
        {
            started = false;
            endTime = DateTime.Now;
            log.Performance(module, function, startTime, endTime, new List<KeyValuePair<string, object>>(), 1, status, additionalMsg);
        }
    }
}