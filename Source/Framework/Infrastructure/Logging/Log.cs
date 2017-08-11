using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using Framework.Infrastructure.Utils;
using System.IO;
using System.Runtime.CompilerServices;
using Framework.Infrastructure.Config;

namespace Framework.Infrastructure.Logging
{
    public class Log : ILog
    {
        private IBaseConfiguration config;
        private readonly NLog.Logger logger = null;

        public Log(IBaseConfiguration config)
        {
            this.config = config;

            var nlogConfig = new LoggingConfiguration();

            if (this.config.LogToFile)
            {
                var fileTarget = new FileTarget();
                nlogConfig.AddTarget("file", fileTarget);
                fileTarget.FileName = config.LogLocation + "/${shortdate}.log";
                fileTarget.Layout = "${longdate}\t${event-context:item=severity}\t${processid}\t${threadid}\t${event-context:item=current-function}\t${event-context:item=current-source-file-name}\t${event-context:item=current-source-line-number}\t${event-context:item=elapsed-time}\t${event-context:item=result}\t${message}";
                fileTarget.ConcurrentWrites = true;

                //fileTarget.DeleteOldFileOnStartup = true;
                fileTarget.ArchiveEvery = FileArchivePeriod.Month;
                fileTarget.MaxArchiveFiles = 30;

                var rule1 = new LoggingRule("*", LogLevel.Trace, fileTarget);
                nlogConfig.LoggingRules.Add(rule1);
            }

            if (this.config.LogToConsole)
            {
                var consoleTarget = new ColoredConsoleTarget();
                consoleTarget.Layout = "${longdate}\t${event-context:item=severity}\t${message}";
                consoleTarget.UseDefaultRowHighlightingRules = true;

                var rule3 = new LoggingRule("*", LogLevel.Trace, consoleTarget);
                nlogConfig.LoggingRules.Add(rule3);
            }

            LogManager.Configuration = nlogConfig;

            logger = LogManager.GetLogger("AppLog");
        }

        public void LogEvent(string severity, LogLevel logLevel, string str, string elapsedTime, string result, int sourceLineNumber, string memberName, string sourceFilePath)
        {
            var theEvent = new LogEventInfo { Level = logLevel, Message = StringUtils.FlattenString(str) };
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
            if (config.LogTraceEnable)
                this.LogEvent("TRACE", LogLevel.Trace, str,"","", sourceLineNumber, memberName, sourceFilePath);
        }
        public void Debug(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (config.LogDebugEnable)
                this.LogEvent("DEBUG", LogLevel.Debug, str, "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Info(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (config.LogInfoEnable)
                this.LogEvent("INFO", LogLevel.Info, str, "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Warn(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (config.LogWarnEnable)
                this.LogEvent("WARNING", LogLevel.Warn, str, "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Error(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent("ERROR", LogLevel.Error, str, "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Error(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent("ERROR", LogLevel.Error, $"{str} Exception - {ex.RecursivelyGetExceptionMessage()}", "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Error(Exception ex, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent("ERROR", LogLevel.Error, $"Exception - {ex.RecursivelyGetExceptionMessage()}", "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Fatal(string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent("FATAL", LogLevel.Fatal, str, "", "", sourceLineNumber, memberName, sourceFilePath);
        }
        public void Fatal(Exception ex, string str, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent("FATAL", LogLevel.Error, $"{str} Exception - {ex.RecursivelyGetExceptionMessage()}", "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Fatal(Exception ex, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            this.LogEvent("FATAL", LogLevel.Error, $"Exception - {ex.RecursivelyGetExceptionMessage()}", "", "", sourceLineNumber, memberName, sourceFilePath);
        }
        public void SqlBeginTransaction(int count, bool functionCalled, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (config.LogSqlEnable)
                this.LogEvent("SQL-BEGIN-TRANSACTION", LogLevel.Info, (functionCalled ? "Called" : ("Increment Count - " + count.ToString())), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlCommitTransaction(int count, bool functionCalled, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (config.LogSqlEnable)
                this.LogEvent("SQL-COMMIT-TRANSACTION", LogLevel.Info, (functionCalled ? "Called" : (" Decrement Count - " + count.ToString())), "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlRollbackTransaction(int count, bool functionCalled, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (config.LogSqlEnable)
                LogEvent("SQL-ROLLBACK-TRANSACTION", LogLevel.Info, (functionCalled ? "Called" : (" Decrement Count - " + count.ToString())), "", "", sourceLineNumber, memberName, sourceFilePath);
        }

        public void Sql(string sqlStr, string sqlResult, TimeSpan ts, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (config.LogSqlEnable)
                this.LogEvent("SQL", LogLevel.Info, sqlStr, (ts.TotalMilliseconds / 1000).ToString(), sqlResult,  sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlError(Exception ex, string sqlStr, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (config.LogSqlEnable)
                this.LogEvent("SQL-ERROR", LogLevel.Info, String.Format("{0} SQL - {1}", ExceptionUtils.RecursivelyGetExceptionMessage(ex), sqlStr), string.Empty, string.Empty, sourceLineNumber, memberName, sourceFilePath);
        }

        public void SqlError(string sqlStr, [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
        {
            if (config.LogSqlEnable)
                this.LogEvent("SQL-ERROR", LogLevel.Info, sqlStr, "", "", sourceLineNumber, memberName, sourceFilePath);
        }
    }
}