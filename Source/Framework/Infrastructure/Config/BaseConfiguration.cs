using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        protected void PopulateFromConfigFile(Configuration config, string configLocation)
        {
            LogTraceEnable = config.AppSettings.Settings["LogTraceEnable"] != null ? SafeUtils.Bool(config.AppSettings.Settings["LogTraceEnable"].Value) : LogTraceEnable;
            LogDebugEnable = config.AppSettings.Settings["LogDebugEnable"] != null ? SafeUtils.Bool(config.AppSettings.Settings["LogDebugEnable"].Value) : LogDebugEnable;
            LogInfoEnable = config.AppSettings.Settings["LogInfoEnable"] != null ? SafeUtils.Bool(config.AppSettings.Settings["LogInfoEnable"].Value) : LogInfoEnable;
            LogSqlEnable = config.AppSettings.Settings["LogSqlEnable"] != null ? SafeUtils.Bool(config.AppSettings.Settings["LogSqlEnable"].Value) : LogSqlEnable;
            LogWarnEnable = config.AppSettings.Settings["LogWarnEnable"] != null ? SafeUtils.Bool(config.AppSettings.Settings["LogWarnEnable"].Value) : LogWarnEnable;

            LogLocation = config.AppSettings.Settings["LogLocation"] != null ? config.AppSettings.Settings["LogLocation"].Value : LogLocation;
            LogLocation = LogLocation.Replace("|ConfigPath|", FileUtils.GetFileDirectory(configLocation));
            LogLocation = Path.GetFullPath((new Uri(LogLocation)).LocalPath);

            DatabaseName = config.AppSettings.Settings["DatabaseName"] != null ? config.AppSettings.Settings["DatabaseName"].Value : DatabaseName;
            DatabaseName = DatabaseName.Replace("|ConfigPath|", FileUtils.GetFileDirectory(configLocation));
            DatabaseName = Path.GetFullPath((new Uri(DatabaseName)).LocalPath);

            DatabaseType = config.AppSettings.Settings["DatabaseType"] != null ? config.AppSettings.Settings["DatabaseType"].Value : DatabaseType;
            DatabaseServer = config.AppSettings.Settings["DatabaseServer"] != null ? config.AppSettings.Settings["DatabaseServer"].Value : DatabaseServer; ;
            DatabaseUserName = config.AppSettings.Settings["DatabaseUserName"] != null ? config.AppSettings.Settings["DatabaseUserName"].Value : DatabaseUserName; ;
            DatabasePassword = config.AppSettings.Settings["DatabasePassword"] != null ? config.AppSettings.Settings["DatabasePassword"].Value : DatabasePassword;
            DatabaseCommandTimeout = config.AppSettings.Settings["DatabaseCommandTimeout"] != null ? SafeUtils.Int(config.AppSettings.Settings["DatabaseCommandTimeout"].Value) : DatabaseCommandTimeout;

            AppName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        }
    }
}
