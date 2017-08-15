using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using Framework.Infrastructure.Utils;
using System.IO;
using System.Runtime.CompilerServices;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Models.Config;
using System.Collections.Generic;

namespace Framework.Infrastructure.Logging
{
    public class Log : ILog
    {
        private LogSettings logConfig;
        private readonly NLog.Logger logger = null;
        private readonly NLog.Logger perfLogger = null;
        private const string AppLoggerName = "AppLog";
        private const string PerfLoggerName = "PerfLog";
        private IBaseConfiguration baseConfig;
        private const string AppFileLayout = "${longdate}\t${event-context:item=severity}\t${processid}\t${threadid}\t${event-context:item=current-function}\t${event-context:item=current-source-file-name}\t${event-context:item=current-source-line-number}\t${event-context:item=elapsed-time}\t${event-context:item=result}\t${message}";
        private const string PerfConsoleLayout = "${time} ${event-context:item=app-module} ${event-context:item=app-function} ";
        private const string PerfFileLayout = "${longdate}\t${event-context:item=app-function}\t${event-context:item=start-time\t${event-context:item=end-time\t${event-context:item=elapsed-time\t${event-context:item=parameters\t${event-context:item=status";
        private const string AppConsoleLayout = "${time} [${event-context:item=severity}] ${message}";

        public Log(IBaseConfiguration baseConfig)
        {
            this.baseConfig = baseConfig;
            this.logConfig = baseConfig.LogSettings;

            var nlogConfig = new LoggingConfiguration();

            if (this.logConfig.LogToFile)
            {
                var fileTarget = new FileTarget();
                fileTarget.FileName = this.logConfig.LogLocation + "/"+ baseConfig.AppName + "/AppLogs/${shortdate}.log";
                fileTarget.Layout = AppFileLayout;
                fileTarget.ConcurrentWrites = true;

                //fileTarget.DeleteOldFileOnStartup = true;
                fileTarget.ArchiveEvery = FileArchivePeriod.Month;
                fileTarget.MaxArchiveFiles = 30;

                var rule1 = new LoggingRule(AppLoggerName, LogLevel.Trace, fileTarget);
                nlogConfig.LoggingRules.Add(rule1);

                if(this.logConfig.LogPerformance)
                {
                    fileTarget = new FileTarget();
                    fileTarget.FileName = this.logConfig.LogLocation + "/" + baseConfig.AppName + "/PerfLogs/${shortdate}.log";

                    fileTarget.Layout = PerfFileLayout;
                    fileTarget.ConcurrentWrites = true;

                    //fileTarget.DeleteOldFileOnStartup = true;
                    fileTarget.ArchiveEvery = FileArchivePeriod.Month;
                    fileTarget.MaxArchiveFiles = 30;

                    var perfRule1 = new LoggingRule(PerfLoggerName, LogLevel.Trace, fileTarget);
                    nlogConfig.LoggingRules.Add(perfRule1);
                }
            }

            if (this.logConfig.LogToDebugger)
            {
                var debugTarget = new NLogDebugTarget();
                debugTarget.Layout = AppFileLayout;
                var rule2 = new LoggingRule(AppLoggerName, LogLevel.Trace, debugTarget);
                nlogConfig.LoggingRules.Add(rule2);
                if (this.logConfig.LogPerformance)
                {
                    debugTarget = new NLogDebugTarget();
                    debugTarget.Layout = PerfFileLayout;
                    var perfRule2 = new LoggingRule(PerfLoggerName, LogLevel.Trace, debugTarget);
                    nlogConfig.LoggingRules.Add(perfRule2);
                }
            }

            if (this.logConfig.LogToConsole)
            {
                var consoleTarget = new ColoredConsoleTarget();
                consoleTarget.Layout = AppConsoleLayout;
                consoleTarget.UseDefaultRowHighlightingRules = true;

                var rule3 = new LoggingRule(AppLoggerName, LogLevel.Trace, consoleTarget);
                nlogConfig.LoggingRules.Add(rule3);

                if (this.logConfig.LogPerformance)
                {
                    consoleTarget = new ColoredConsoleTarget();
                    
                    consoleTarget.Layout = PerfConsoleLayout;
                    consoleTarget.UseDefaultRowHighlightingRules = true;
                    var perfRule3 = new LoggingRule(PerfLoggerName, LogLevel.Trace, consoleTarget);
                    nlogConfig.LoggingRules.Add(perfRule3);
                }
            }
            

            LogManager.Configuration = nlogConfig;

            logger = LogManager.GetLogger(AppLoggerName);
            perfLogger = LogManager.GetLogger(PerfLoggerName);
        }

        {
            var theEvent = new LogEventInfo { Level = LogLevel.Debug };
            theEvent.Properties.Add("app-name", this.baseConfig.AppName);
            theEvent.Properties.Add("app-module", appModule);
            theEvent.Properties.Add("app-function", appFunction);
            theEvent.Properties.Add("start-time", startTime);
            theEvent.Properties.Add("end-time", endTime);
            theEvent.Properties.Add("elapsed-time-ms", (endTime - startTime).TotalMilliseconds);
            theEvent.Properties.Add("parameters", JsonUtils.Serialize(parameters));
            theEvent.Properties.Add("statusCode", statusCode);
            theEvent.Properties.Add("status", status);
            theEvent.Properties.Add("current-function", memberName ?? "");
            theEvent.Properties.Add("current-source-line-number", sourceLineNumber);
            theEvent.Properties.Add("current-source-file-name", Path.GetFileName(sourceFilePath ?? ""));
            theEvent.Message = additionalMsg;
            theEvent.TimeStamp = DateTime.Now;
            perfLogger.Log(theEvent);
        }

        private void LogEvent(string severity, LogLevel logLevel, string str, string elapsedTime, string result, int sourceLineNumber, string memberName, string sourceFilePath)
        {
            var theEvent = new LogEventInfo { Level = logLevel, Message = StringUtils.FlattenString(str) };
            theEvent.Properties.Add("app-name", this.baseConfig.AppName);
            theEvent.Properties.Add("severity", severity);
            theEvent.Properties.Add("current-function", memberName ?? "");
            theEvent.Properties.Add("current-source-line-number", sourceLineNumber);
            theEvent.Properties.Add("current-source-file-name", Path.GetFileName(sourceFilePath ?? ""));
            theEvent.Properties.Add("result", result);
            theEvent.Properties.Add("elapsed-time", elapsedTime);
            theEvent.TimeStamp = DateTime.Now;
            logger.Log(theEvent);
        }

        public void Trace(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogTrace)
                this.LogEvent("TRACE", LogLevel.Trace, str,"","", sourceLineNumber, memberName, sourceFilePath);
        }
        public void Trace(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogTrace)
                this.LogEvent("TRACE", LogLevel.Trace, $"{str} " + (ex != null ? $"Exception - { ex.RecursivelyGetExceptionMessage()}" : ""), "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Debug(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogDebug)
                this.LogEvent("DEBUG", LogLevel.Debug, str, "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Debug(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogDebug)
                this.LogEvent("DEBUG", LogLevel.Debug, $"{str} " + (ex != null ? $"Exception - { ex.RecursivelyGetExceptionMessage()}" : ""), "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Info(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogInfo)
                this.LogEvent("INFO", LogLevel.Info, str, "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Info(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogInfo)
                this.LogEvent("INFO", LogLevel.Info, $"{str} " + (ex != null ? $"Exception - { ex.RecursivelyGetExceptionMessage()}" : ""), "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Warn(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogWarn)
                this.LogEvent("WARNING", LogLevel.Warn, str, "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Warn(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogWarn)
                this.LogEvent("WARNING", LogLevel.Warn, $"{str} " + (ex != null ? $"Exception - { ex.RecursivelyGetExceptionMessage()}" : ""), "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Error(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if(logConfig.LogError)
                this.LogEvent("ERROR", LogLevel.Error, str, "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Error(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogError)
                this.LogEvent("ERROR", LogLevel.Error, $"{str} " + (ex != null ? $"Exception - { ex.RecursivelyGetExceptionMessage()}" : ""), "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Error(Exception ex, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogError)
                this.LogEvent("ERROR", LogLevel.Error, $"Exception - {ex.RecursivelyGetExceptionMessage()}", "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Fatal(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent("FATAL", LogLevel.Fatal, str, "", "", sourceLineNumber, memberName, sourceFilePath);
        }
        public void Fatal(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent("FATAL", LogLevel.Error, $"{str} " + (ex != null ? $"Exception - { ex.RecursivelyGetExceptionMessage()}" : ""), "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Fatal(Exception ex, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent("FATAL", LogLevel.Error, $"Exception - {ex.RecursivelyGetExceptionMessage()}", "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlBeginTransaction(int count, bool functionCalled, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogSql)
                this.LogEvent("SQL-BEGIN-TRANSACTION", LogLevel.Info, (functionCalled ? "Called" : ("Increment Count - " + count.ToString())), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlCommitTransaction(int count, bool functionCalled, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogSql)
                this.LogEvent("SQL-COMMIT-TRANSACTION", LogLevel.Info, (functionCalled ? "Called" : (" Decrement Count - " + count.ToString())), "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlRollbackTransaction(int count, bool functionCalled, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogSql)
                LogEvent("SQL-ROLLBACK-TRANSACTION", LogLevel.Info, (functionCalled ? "Called" : (" Decrement Count - " + count.ToString())), "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Sql(string sqlStr, string sqlResult, TimeSpan ts, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogSql)
                this.LogEvent("SQL", LogLevel.Info, sqlStr, (ts.TotalMilliseconds / 1000).ToString(), sqlResult,  sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlError(Exception ex, string sqlStr, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogError)
                this.LogEvent("SQL-ERROR", LogLevel.Info, String.Format("{0} SQL - {1}", ExceptionUtils.RecursivelyGetExceptionMessage(ex), sqlStr), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlError(string sqlStr, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (logConfig.LogError)
                this.LogEvent("SQL-ERROR", LogLevel.Info, sqlStr, "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        {
            if (logConfig.LogPerformance)
                this.LogPerfEvent(appModule, appFunction, startTime, endTime, parameters, statusCode, status, additionalMsg, sourceLineNumber, memberName, sourceFilePath);
        }

    }
}