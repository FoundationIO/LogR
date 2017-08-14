using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Config
{
    public class BaseConfiguration : IBaseConfiguration
    {
        //General App Related
        public string AppName { get; set; } = null;
        public string MigrationNamespace { get; set; } = null;


        //log related
        public bool LogTrace { get; private set; }
        public bool LogDebug { get; private set; }
        public bool LogInfo { get; private set; }
        public bool LogSql { get; private set; }
        public bool LogWarn { get; private set; }
        public bool LogError { get; private set; }

        public bool LogToFile { get; private set; }
        public bool LogToDebugger { get; private set; }
        public bool LogToConsole { get; private set; }

        public String LogLocation { get; private set; }

        //database related
        public string DatabaseType { get; private set; }
        public string DatabaseName { get; private set; }
        public string DatabaseServer { get; private set; }
        public string DatabaseUserName { get; private set; }
        public string DatabasePassword { get; private set; }
        public int DatabaseCommandTimeout { get; private set; }
        public int MaxPoolSize { get; private set; }

        public BaseConfiguration()
        {
            LogTrace = true;
            LogDebug = true;
            LogInfo = true;
            LogSql = true;
            LogWarn = true;
            LogLocation = System.IO.Directory.GetCurrentDirectory() + "\\" + "Logs";
            MaxPoolSize = 100;
        }

        protected void PrepareFolders()
        {
            if (Directory.Exists(LogLocation) == false)
            {
                Directory.CreateDirectory(LogLocation);
            }

            if (DatabaseType == DBType.SQLITE3)
            {
                if (Directory.Exists(FileUtils.GetFileDirectory(DatabaseName)) == false)
                    Directory.CreateDirectory(FileUtils.GetFileDirectory(DatabaseName));
            }
        }

        protected  virtual String GetConfigFileLocation()
        {
            return "";
        }

        protected void PopulateFromConfigFile(IConfigurationSection appSettings, string configLocation)
        {
            LogTrace = SafeUtils.Bool(appSettings["logTrace"], LogTrace);
            LogDebug = SafeUtils.Bool(appSettings["logDebug"], LogDebug);
            LogInfo = SafeUtils.Bool(appSettings["logInfo"], LogInfo);
            LogSql = SafeUtils.Bool(appSettings["logSql"], LogSql);
            LogWarn = SafeUtils.Bool(appSettings["logWarn"], LogWarn);
            LogError = SafeUtils.Bool(appSettings["logWarn"], LogError);

            LogToFile = SafeUtils.Bool(appSettings["logToFile"], LogToFile);
            LogToDebugger = SafeUtils.Bool(appSettings["logToDebugger"], LogToDebugger);
            LogToConsole = SafeUtils.Bool(appSettings["logToConsole"], LogToConsole);

            LogLocation = appSettings["logLocation"] ?? LogLocation;
            if (LogLocation.IsTrimmedStringNotNullOrEmpty() && LogLocation.Contains("|ConfigPath|") )
            {
                LogLocation = LogLocation.Replace("|ConfigPath|", FileUtils.GetFileDirectory(configLocation));
                LogLocation = Path.GetFullPath(new Uri(LogLocation).LocalPath);
            }

            DatabaseName = appSettings["databaseName"] ?? DatabaseName;
            if (DatabaseName.IsTrimmedStringNotNullOrEmpty() && LogLocation.Contains("|ConfigPath|"))
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
            MigrationNamespace = appSettings["MigrationNamespace"] ?? "";
            
            AppName = Path.GetFileNameWithoutExtension(GetType().GetTypeInfo().Assembly.Location);            
        }
    }
}
