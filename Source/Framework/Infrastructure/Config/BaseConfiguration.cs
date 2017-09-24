using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Models.Config;
using Framework.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;

namespace Framework.Infrastructure.Config
{
    public class BaseConfiguration : IBaseConfiguration
    {
        public BaseConfiguration()
        {
            MaxPoolSize = 100;
        }

        //General App Related
        public string AppName { get; set; } = null;

        public string MigrationNamespace { get; set; } = null;

        // log related
        public LogSettings LogSettings { get; private set; }

        //database related
        public string DatabaseType { get; private set; }

        public string DatabaseName { get; private set; }

        public string DatabaseServer { get; private set; }

        public string DatabaseUserName { get; private set; }

        public string DatabasePassword { get; private set; }

        public bool DatabaseUseIntegratedLogin { get; private set; }

        public int DatabaseCommandTimeout { get; private set; }

        public int MaxPoolSize { get; private set; }

        protected void PrepareFolders()
        {
            if (Directory.Exists(LogSettings.LogLocation) == false)
            {
                Directory.CreateDirectory(LogSettings.LogLocation);
            }

            if (DatabaseType == DBType.SQLITE3)
            {
                if (Directory.Exists(FileUtils.GetFileDirectory(DatabaseName)) == false)
                    Directory.CreateDirectory(FileUtils.GetFileDirectory(DatabaseName));
            }
        }

        protected virtual string GetConfigFileLocation()
        {
            return string.Empty;
        }

        protected void PopulateFromConfigFile(IConfigurationSection appSettings, IConfigurationSection frameworkLogSetting, string configLocation)
        {
            var logTrace = SafeUtils.Bool(appSettings["logTrace"], true);
            var logDebug = SafeUtils.Bool(appSettings["logDebug"], true);
            var logInfo = SafeUtils.Bool(appSettings["logInfo"], true);
            var logSql = SafeUtils.Bool(appSettings["logSql"], true);
            var logWarn = SafeUtils.Bool(appSettings["logWarn"], true);
            var logError = SafeUtils.Bool(appSettings["logError"], true);
            var logPerformance = SafeUtils.Bool(appSettings["logPerformance"], true);

            var logToFile = SafeUtils.Bool(appSettings["logToFile"], true);
            var logToDebugger = SafeUtils.Bool(appSettings["logToDebugger"], true);
            var logToConsole = SafeUtils.Bool(appSettings["logToConsole"], true);

            var logLocation = appSettings["logLocation"] ?? FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "Logs");
            if (logLocation.IsTrimmedStringNotNullOrEmpty() && logLocation.Contains("|ConfigPath|"))
            {
                logLocation = logLocation.Replace("|ConfigPath|", FileUtils.GetFileDirectory(configLocation));
                logLocation = Path.GetFullPath(new Uri(logLocation).LocalPath);
            }

            //Load .Net core's Logging configuration
            var otherLogSettings = new List<KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>>();
            var logLevelSettings = frameworkLogSetting.GetSection("LogLevel");
            foreach (var setting in logLevelSettings.GetChildren())
            {
                var logLevel = SafeUtils.Enum<Microsoft.Extensions.Logging.LogLevel>(setting.Value, Microsoft.Extensions.Logging.LogLevel.None);
                otherLogSettings.Add(new KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>(setting.Key, logLevel));
            }

            LogSettings = new LogSettings(logTrace, logDebug, logInfo, logSql, logWarn, logError, logLocation, logToFile, logToConsole, logToDebugger, logPerformance, otherLogSettings);

            DatabaseName = appSettings["databaseName"] ?? DatabaseName;
            if (DatabaseName.IsTrimmedStringNotNullOrEmpty() && DatabaseName.Contains("|ConfigPath|"))
            {
                DatabaseName = DatabaseName.Replace("|ConfigPath|", FileUtils.GetFileDirectory(configLocation));
                DatabaseName = Path.GetFullPath(new Uri(DatabaseName).LocalPath);
            }

            DatabaseType = appSettings["databaseType"] ?? DatabaseType;
            DatabaseServer = appSettings["databaseServer"] ?? DatabaseServer;
            DatabaseUserName = appSettings["databaseUserName"] ?? DatabaseUserName;
            DatabasePassword = appSettings["databasePassword"] ?? DatabasePassword;
            DatabaseCommandTimeout = SafeUtils.Int(appSettings["databaseCommandTimeout"], DatabaseCommandTimeout);
            MaxPoolSize = SafeUtils.Int(appSettings["maxPoolSize"], MaxPoolSize);
            MigrationNamespace = appSettings["migrationNamespace"] ?? string.Empty;

            AppName = Path.GetFileNameWithoutExtension(GetType().GetTypeInfo().Assembly.Location);
        }
    }
}
