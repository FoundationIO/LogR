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

        public bool AutomaticMigration { get; private set; }

        public string MigrationNamespace { get; set; } = null;

        public string MigrationProfile { get; set; } = null;

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
            var logTrace = SafeUtils.Bool(appSettings[Strings.Config.LogTrace], true);
            var logDebug = SafeUtils.Bool(appSettings[Strings.Config.LogDebug], true);
            var logInfo = SafeUtils.Bool(appSettings[Strings.Config.LogInfo], true);
            var logSql = SafeUtils.Bool(appSettings[Strings.Config.LogSql], true);
            var logWarn = SafeUtils.Bool(appSettings[Strings.Config.LogWarn], true);
            var logError = SafeUtils.Bool(appSettings[Strings.Config.LogError], true);
            var logPerformance = SafeUtils.Bool(appSettings[Strings.Config.LogPerformance], true);

            var logToFile = SafeUtils.Bool(appSettings[Strings.Config.LogToFile], true);
            var logToDebugger = SafeUtils.Bool(appSettings[Strings.Config.LogToDebugger], true);
            var logToConsole = SafeUtils.Bool(appSettings[Strings.Config.LogToConsole], true);

            var logLocation = appSettings[Strings.Config.LogLocation] ?? FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "Logs");
            if (logLocation.IsTrimmedStringNotNullOrEmpty() && logLocation.Contains(Strings.Config.ConfigPath))
            {
                logLocation = logLocation.Replace(Strings.Config.ConfigPath, FileUtils.GetFileDirectory(configLocation));
                logLocation = Path.GetFullPath(new Uri(logLocation).LocalPath);
            }

            //Load .Net core's Logging configuration
            var otherLogSettings = new List<KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>>();
            var logLevelSettings = frameworkLogSetting.GetSection(Strings.Config.LogLevel);
            foreach (var setting in logLevelSettings.GetChildren())
            {
                var logLevel = SafeUtils.Enum<Microsoft.Extensions.Logging.LogLevel>(setting.Value, Microsoft.Extensions.Logging.LogLevel.None);
                otherLogSettings.Add(new KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>(setting.Key, logLevel));
            }

            LogSettings = new LogSettings(logTrace, logDebug, logInfo, logSql, logWarn, logError, logLocation, logToFile, logToConsole, logToDebugger, logPerformance, otherLogSettings);

            DatabaseName = appSettings[Strings.Config.DatabaseName] ?? DatabaseName;
            if (DatabaseName.IsTrimmedStringNotNullOrEmpty() && DatabaseName.Contains(Strings.Config.ConfigPath))
            {
                DatabaseName = DatabaseName.Replace(Strings.Config.ConfigPath, FileUtils.GetFileDirectory(configLocation));
                DatabaseName = Path.GetFullPath(new Uri(DatabaseName).LocalPath);
            }

            DatabaseType = appSettings[Strings.Config.DatabaseType] ?? DatabaseType;
            DatabaseServer = appSettings[Strings.Config.DatabaseServer] ?? DatabaseServer;
            DatabaseUserName = appSettings[Strings.Config.DatabaseUserName] ?? DatabaseUserName;
            DatabasePassword = appSettings[Strings.Config.DatabasePassword] ?? DatabasePassword;
            DatabaseCommandTimeout = SafeUtils.Int(appSettings[Strings.Config.DatabaseCommandTimeout], DatabaseCommandTimeout);
            MaxPoolSize = SafeUtils.Int(appSettings[Strings.Config.MaxPoolSize], MaxPoolSize);
            MigrationNamespace = appSettings[Strings.Config.MigrationNamespace] ?? string.Empty;
            AutomaticMigration = SafeUtils.Bool(appSettings[Strings.Config.AutomaticMigration],false);
            MigrationProfile = null; // Always null and if App wants to use any other profile, they are free to do so

            AppName = Path.GetFileNameWithoutExtension(GetType().GetTypeInfo().Assembly.Location);
        }
    }
}
