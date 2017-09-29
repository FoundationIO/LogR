using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Models.Config;
using Framework.Infrastructure.Utils;
using NLog.Config;
using NLog.Targets;

namespace Framework.Infrastructure.Logging
{
    public class Log : ILog
    {
        private const string AppLoggerName = "AppLog";
        private const string PerfLoggerName = "PerfLog";
        private const string AppFileLayout = "${longdate}\t${event-context:item=severity}\t${processid}\t${threadid}\t${event-context:item=current-function}\t${event-context:item=current-source-file-name}\t${event-context:item=current-source-line-number}\t${event-context:item=elapsed-time}\t${event-context:item=result}\t${message}";
        private const string PerfConsoleLayout = "${time} ${event-context:item=app-module} ${event-context:item=app-function} ";
        private const string PerfFileLayout = "${longdate}\t${event-context:item=app-function}\t${event-context:item=start-time\t${event-context:item=end-time\t${event-context:item=elapsed-time\t${event-context:item=parameters\t${event-context:item=status";
        private const string AppConsoleLayout = "${time} [${event-context:item=severity}] ${message}";

        private LogSettings logConfig;
        private NLog.Logger logger = null;
        private NLog.Logger perfLogger = null;
        private IBaseConfiguration baseConfig;

        public Log(IBaseConfiguration baseConfig)
        {
            this.baseConfig = baseConfig;
            this.logConfig = baseConfig.LogSettings;

            var nlogConfig = new LoggingConfiguration();

            if (this.logConfig.LogToFile)
            {
                var fileTarget = new FileTarget
                {
                    FileName = this.logConfig.LogLocation + "/" + baseConfig.AppName + "/AppLogs/${shortdate}.log",
                    Layout = AppFileLayout,
                    ConcurrentWrites = true,

                    ArchiveEvery = FileArchivePeriod.Month,
                    MaxArchiveFiles = 30
                };

                var rule1 = new LoggingRule(AppLoggerName, NLog.LogLevel.Trace, fileTarget);
                nlogConfig.LoggingRules.Add(rule1);

                if (this.logConfig.LogPerformance)
                {
                    fileTarget = new FileTarget
                    {
                        FileName = this.logConfig.LogLocation + "/" + baseConfig.AppName + "/PerfLogs/${shortdate}.log",

                        Layout = PerfFileLayout,
                        ConcurrentWrites = true,

                        ArchiveEvery = FileArchivePeriod.Month,
                        MaxArchiveFiles = 30
                    };

                    var perfRule1 = new LoggingRule(PerfLoggerName, NLog.LogLevel.Trace, fileTarget);
                    nlogConfig.LoggingRules.Add(perfRule1);
                }
            }

            if (this.logConfig.LogToDebugger)
            {
                var debugTarget = new NLogDebugTarget
                {
                    Layout = AppFileLayout
                };
                var rule2 = new LoggingRule(AppLoggerName, NLog.LogLevel.Trace, debugTarget);
                nlogConfig.LoggingRules.Add(rule2);
                if (this.logConfig.LogPerformance)
                {
                    debugTarget = new NLogDebugTarget
                    {
                        Layout = PerfFileLayout
                    };
                    var perfRule2 = new LoggingRule(PerfLoggerName, NLog.LogLevel.Trace, debugTarget);
                    nlogConfig.LoggingRules.Add(perfRule2);
                }
            }

            if (this.logConfig.LogToConsole)
            {
                var consoleTarget = new ColoredConsoleTarget
                {
                    Layout = AppConsoleLayout,
                    UseDefaultRowHighlightingRules = true
                };

                var rule3 = new LoggingRule(AppLoggerName, NLog.LogLevel.Trace, consoleTarget);
                nlogConfig.LoggingRules.Add(rule3);

                if (this.logConfig.LogPerformance)
                {
                    consoleTarget = new ColoredConsoleTarget
                    {
                        Layout = PerfConsoleLayout,
                        UseDefaultRowHighlightingRules = true
                    };
                    var perfRule3 = new LoggingRule(PerfLoggerName, NLog.LogLevel.Trace, consoleTarget);
                    nlogConfig.LoggingRules.Add(perfRule3);
                }
            }

            NLog.LogManager.Configuration = nlogConfig;

            logger = NLog.LogManager.GetLogger(AppLoggerName);
            perfLogger = NLog.LogManager.GetLogger(PerfLoggerName);
        }

        public void Trace(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogTrace)
                this.LogEvent(Strings.Log.Trace, NLog.LogLevel.Trace, str, string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Trace(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogTrace)
                this.LogEvent(Strings.Log.Trace, NLog.LogLevel.Trace, $"{str} " + (ex != null ? $"Exception - {ex.RecursivelyGetExceptionMessage()}" : string.Empty), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Debug(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogDebug)
                this.LogEvent(Strings.Log.Debug, NLog.LogLevel.Debug, str, string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Debug(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogDebug)
                this.LogEvent(Strings.Log.Debug, NLog.LogLevel.Debug, $"{str} " + (ex != null ? $"Exception - {ex.RecursivelyGetExceptionMessage()}" : string.Empty), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Info(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogInfo)
                this.LogEvent(Strings.Log.Info, NLog.LogLevel.Info, str, string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Info(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogInfo)
                this.LogEvent(Strings.Log.Info, NLog.LogLevel.Info, $"{str} " + (ex != null ? $"Exception - {ex.RecursivelyGetExceptionMessage()}" : string.Empty), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Warn(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogWarn)
                this.LogEvent(Strings.Log.Warning, NLog.LogLevel.Warn, str, string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Warn(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogWarn)
                this.LogEvent(Strings.Log.Warning, NLog.LogLevel.Warn, $"{str} " + (ex != null ? $"Exception - {ex.RecursivelyGetExceptionMessage()}" : string.Empty), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Error(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogError)
                this.LogEvent(Strings.Log.Error, NLog.LogLevel.Error, str, string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Error(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogError)
                this.LogEvent(Strings.Log.Error, NLog.LogLevel.Error, $"{str} " + (ex != null ? $"Exception - {ex.RecursivelyGetExceptionMessage()}" : string.Empty), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Error(Exception ex, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogError)
                this.LogEvent(Strings.Log.Error, NLog.LogLevel.Error, $"Exception - {ex.RecursivelyGetExceptionMessage()}", string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Fatal(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent(Strings.Log.Fatal, NLog.LogLevel.Fatal, str, string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Fatal(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent(Strings.Log.Fatal, NLog.LogLevel.Fatal, $"{str} " + (ex != null ? $"Exception - {ex.RecursivelyGetExceptionMessage()}" : string.Empty), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Fatal(Exception ex, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent(Strings.Log.Fatal, NLog.LogLevel.Fatal, $"Exception - {ex.RecursivelyGetExceptionMessage()}", string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlBeginTransaction(int count, bool functionCalled, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogSql)
                this.LogEvent(Strings.Log.SqlBeginTransaction, NLog.LogLevel.Info, functionCalled ? "Called" : ("Increment Count - " + count.ToString()), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlCommitTransaction(int count, bool functionCalled, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogSql)
                this.LogEvent(Strings.Log.SqlCommitTransaction, NLog.LogLevel.Info, functionCalled ? "Called" : (" Decrement Count - " + count.ToString()), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlRollbackTransaction(int count, bool functionCalled, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogSql)
                LogEvent(Strings.Log.SqlRollbackTransaction, NLog.LogLevel.Info, functionCalled ? "Called" : (" Decrement Count - " + count.ToString()), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Sql(string sqlStr, string sqlResult, TimeSpan ts, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogSql)
                this.LogEvent(Strings.Log.Sql, NLog.LogLevel.Info, sqlStr, (ts.TotalMilliseconds / 1000).ToString(), sqlResult,  sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlError(Exception ex, string sqlStr, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogError)
                this.LogEvent(Strings.Log.SqlError, NLog.LogLevel.Error, string.Format("{0} SQL - {1}", ExceptionUtils.RecursivelyGetExceptionMessage(ex), sqlStr), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlError(string sqlStr, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogError)
                this.LogEvent(Strings.Log.SqlError, NLog.LogLevel.Info, sqlStr, string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void Performance(string appModule, string appFunction, DateTime startTime, DateTime endTime, List<KeyValuePair<string,object>> parameters, int statusCode , string status, string additionalMsg, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogPerformance)
                this.LogPerfEvent(appModule, appFunction, startTime, endTime, parameters, statusCode, status, additionalMsg, sourceLineNumber, memberName, sourceFilePath);
        }

        private void LogPerfEvent(string appModule, string appFunction, DateTime startTime, DateTime endTime, List<KeyValuePair<string, object>> parameters, int statusCode, string status, string additionalMsg, int sourceLineNumber, string memberName, string sourceFilePath)
        {
            var theEvent = new NLog.LogEventInfo { Level = NLog.LogLevel.Debug };
            theEvent.Properties.Add("app-name", this.baseConfig.AppName);
            theEvent.Properties.Add("app-module", appModule);
            theEvent.Properties.Add("app-function", appFunction);
            theEvent.Properties.Add("start-time", startTime);
            theEvent.Properties.Add("end-time", endTime);
            theEvent.Properties.Add("elapsed-time-ms", (endTime - startTime).TotalMilliseconds);
            theEvent.Properties.Add("parameters", JsonUtils.Serialize(parameters));
            theEvent.Properties.Add("statusCode", statusCode);
            theEvent.Properties.Add("status", status);
            theEvent.Properties.Add("current-function", memberName ?? string.Empty);
            theEvent.Properties.Add("current-source-line-number", sourceLineNumber);
            theEvent.Properties.Add("current-source-file-name", Path.GetFileName(sourceFilePath ?? string.Empty));
            theEvent.Message = additionalMsg;
            theEvent.TimeStamp = DateTime.Now;
            perfLogger.Log(theEvent);
        }

        private void LogEvent(string severity, NLog.LogLevel logLevel, string str, string elapsedTime, string result, int sourceLineNumber, string memberName, string sourceFilePath)
        {
            var theEvent = new NLog.LogEventInfo { Level = logLevel, Message = StringUtils.FlattenString(str) };
            theEvent.Properties.Add("app-name", this.baseConfig.AppName);
            theEvent.Properties.Add("severity", severity);
            theEvent.Properties.Add("current-function", memberName ?? string.Empty);
            theEvent.Properties.Add("current-source-line-number", sourceLineNumber);
            theEvent.Properties.Add("current-source-file-name", Path.GetFileName(sourceFilePath ?? string.Empty));
            theEvent.Properties.Add("result", result);
            theEvent.Properties.Add("elapsed-time", elapsedTime);
            theEvent.TimeStamp = DateTime.Now;
            logger.Log(theEvent);
        }
    }
}