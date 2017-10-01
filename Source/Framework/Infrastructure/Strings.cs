using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Constants
{
    public class Strings
    {
        public class Config
        {
            public const string LogTrace = "logTrace";
            public const string LogDebug = "logDebug";
            public const string LogInfo = "logInfo";
            public const string LogSql = "logSql";
            public const string LogWarn = "logWarn";
            public const string LogError = "logError";
            public const string LogToFile = "logToFile";
            public const string LogLocation = "logLocation";
            public const string ConfigPath = "|ConfigPath|";
            public const string LogPerformance = "logPerformance";
            public const string LogToDebugger = "logToDebugger";
            public const string LogToConsole = "logToConsole";
            public const string DatabaseType = "databaseType";
            public const string DatabaseName = "databaseName";
            public const string DatabaseServer = "databaseServer";
            public const string DatabaseUserName = "databaseUserName";
            public const string DatabasePassword = "databasePassword";
            public const string DatabaseCommandTimeout = "databaseCommandTimeout";
            public const string MaxPoolSize = "maxPoolSize";
            public const string AutomaticMigration = "automaticMigration";
            public const string MigrationNamespace = "migrationNamespace";
            public const string ServerPort = "serverPort";
            public const string LogLevel = "LogLevel";
            public const string DbSettings = "dbSettings";
            public const string LogSettings = "logSettings";
        }

        public class Log
        {
            public const string Critical = "CRITICAL";
            public const string Trace = "TRACE";
            public const string Error = "ERROR";
            public const string Warning = "WARNING";
            public const string Info = "INFO";
            public const string Fatal = "FATAL";
            public const string Debug = "DEBUG";
            public const string SqlBeginTransaction = "SQL-BEGIN-TRANSACTION";
            public const string SqlCommitTransaction = "SQL-COMMIT-TRANSACTION";
            public const string SqlRollbackTransaction = "SQL-ROLLBACK-TRANSACTION";
            public const string Sql = "SQL";
            public const string SqlError = "SQL-ERROR";
        }
    }
}
