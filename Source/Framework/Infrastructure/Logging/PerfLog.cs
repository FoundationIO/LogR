using System;
using System.Diagnostics;
using Framework.Infrastructure.Utils;
using System.Collections.Generic;

namespace Framework.Infrastructure.Logging
{
    public class PerfLog : IDisposable
    {
        private readonly string module = "";
        private readonly string function = "";
        private bool started = false;
        private readonly bool autoCloseIsError = true;
        private bool logToDefaultLogger = false;
        private ILog log;
        private DateTime startTime,endTime;
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

        private void StopAndWriteToLog(string status = "completed", string additionalMsg = "")
        {
            started = false;
            endTime = DateTime.Now;
            log.Performance(module, function,startTime, endTime, new List<KeyValuePair<string, object>>(), 1, status, additionalMsg);
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
    }
}