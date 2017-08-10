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
        public bool LogTraceEnable { get; private set; }
        public bool LogDebugEnable { get; private set; }
        public bool LogInfoEnable { get; private set; }
        public bool LogSqlEnable { get; private set; }
        public bool LogWarnEnable { get; private set; }

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
        
        public BaseConfiguration()
        {
            LogTraceEnable = true;
            LogDebugEnable = true;
            LogInfoEnable = true;
            LogSqlEnable = true;
            LogWarnEnable = true;
            LogLocation = System.IO.Directory.GetCurrentDirectory() + "\\" + "Logs";
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

        protected void PopulateFromConfigFile(IConfigurationRoot config, string configLocation)
        {
            var appSettings = config.GetSection("AppSettings");

            LogTraceEnable = appSettings["LogTraceEnable"] != null ? SafeUtils.Bool(appSettings["LogTraceEnable"]) : LogTraceEnable;
            LogDebugEnable = appSettings["LogDebugEnable"] != null ? SafeUtils.Bool(appSettings["LogDebugEnable"]) : LogDebugEnable;
            LogInfoEnable = appSettings["LogInfoEnable"] != null ? SafeUtils.Bool(appSettings["LogInfoEnable"]) : LogInfoEnable;
            LogSqlEnable = appSettings["LogSqlEnable"] != null ? SafeUtils.Bool(appSettings["LogSqlEnable"]) : LogSqlEnable;
            LogWarnEnable = appSettings["LogWarnEnable"] != null ? SafeUtils.Bool(appSettings["LogWarnEnable"]) : LogWarnEnable;

            this.LogToFile = appSettings["LogToFile"] != null ? SafeUtils.Bool(appSettings["LogToFile"]) : this.LogToFile;
            this.LogToDebugger = appSettings["LogToDebugger"] != null ? SafeUtils.Bool(appSettings["LogToDebugger"]) : this.LogToDebugger;
            this.LogToConsole = appSettings["LogToConsole"] != null ? SafeUtils.Bool(appSettings["LogToConsole"]) : this.LogToConsole;

            LogLocation = appSettings["LogLocation"] != null ? appSettings["LogLocation"] : LogLocation;
            LogLocation = LogLocation.Replace("|ConfigPath|", FileUtils.GetFileDirectory(configLocation));
            LogLocation = Path.GetFullPath((new Uri(LogLocation)).LocalPath);

            DatabaseName = appSettings["DatabaseName"] != null ? appSettings["DatabaseName"] : DatabaseName;
            DatabaseName = DatabaseName.Replace("|ConfigPath|", FileUtils.GetFileDirectory(configLocation));
            DatabaseName = Path.GetFullPath((new Uri(DatabaseName)).LocalPath);

            DatabaseType = appSettings["DatabaseType"] != null ? appSettings["DatabaseType"] : DatabaseType;
            DatabaseServer = appSettings["DatabaseServer"] != null ? appSettings["DatabaseServer"] : DatabaseServer; ;
            DatabaseUserName = appSettings["DatabaseUserName"] != null ? appSettings["DatabaseUserName"] : DatabaseUserName; ;
            DatabasePassword = appSettings["DatabasePassword"] != null ? appSettings["DatabasePassword"] : DatabasePassword;
            DatabaseCommandTimeout = appSettings["DatabaseCommandTimeout"] != null ? SafeUtils.Int(appSettings["DatabaseCommandTimeout"]) : DatabaseCommandTimeout;

            MigrationNamespace = appSettings["MigrationNamespace"] != null ? appSettings["MigrationNamespace"] : "";

            AppName = Path.GetFileNameWithoutExtension(this.GetType().GetTypeInfo().Assembly.Location);
        }
    }
}
