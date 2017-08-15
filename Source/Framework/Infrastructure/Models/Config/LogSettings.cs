﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Infrastructure.Models.Config
{
    public class LogSettings
    {
        //log related
        public bool LogTrace { get; private set; }
        public bool LogDebug { get; private set; }
        public bool LogInfo { get; private set; }
        public bool LogSql { get; private set; }
        public bool LogWarn { get; private set; }
        public bool LogError { get; private set; }
        public bool LogPerformance { get; private set; }
        public String LogLocation { get; private set; }

        public bool LogToFile { get; private set; }
        public bool LogToConsole { get; private set; }
        public bool LogToDebugger { get; private set; }

        public List<KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>> OtherFrameworkLogSettings { get; private set; }

        public LogSettings(bool logTrace,
                            bool logDebug,
                            bool logInfo,
                            bool logSql,
                            bool logWarn,
                            bool logError,
                            String logLocation,
                            bool logToFile,
                            bool logToConsole,
                            bool logToDebugger,
                            bool logPerformance,
                            List<KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>> otherFrameworkLogSettings)
        {
            LogTrace = logTrace;
            LogDebug = logDebug;
            LogInfo = logInfo;
            LogWarn = logWarn;
            LogError = logError;
            LogLocation = logLocation;
            LogToFile = logToFile;
            LogToConsole = logToConsole;
            LogToDebugger = logToDebugger;
            LogPerformance = logPerformance;
            OtherFrameworkLogSettings = otherFrameworkLogSettings;
        }

        public static LogSettings NoOpLogSettings()
        {
            return new LogSettings(false, false, false, false, false, false, "", false, false, false, false, null);
        }
    }
}