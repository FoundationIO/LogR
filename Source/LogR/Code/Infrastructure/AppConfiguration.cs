using Framework.Contants;
using Framework.Infrastructure;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Code.Infrastructure
{
    public class AppConfiguration : BaseConfiguration , IAppConfiguration
    {
        public AppConfiguration()
        {
            PopulateFromConfigFile();
            PrepareFolders();
        }

        public int ServerPort { get; private set; }

        public String IndexBaseFolder { get; private set; }

        public String AppLogIndexFolder
        {
            get
            {
                return IndexBaseFolder + "\\" + "app_log_index\\";
            }
        }

        public String PerformanceLogIndexFolder
        {
            get
            {
                return IndexBaseFolder + "\\" + "performance_log_index\\";
            }
        }

        protected new void PrepareFolders()
        {
            base.PrepareFolders();

            if (Directory.Exists(LogLocation) == false)
            {
                Directory.CreateDirectory(LogLocation);
            }

            if (Directory.Exists(IndexBaseFolder) == false)
            {
                Directory.CreateDirectory(IndexBaseFolder);
            }
            

            if (DatabaseType == DBType.SQLITE3)
            {
                if (Directory.Exists(FileUtils.GetFileDirectory(DatabaseName)) == false)
                    Directory.CreateDirectory(FileUtils.GetFileDirectory(DatabaseName));
            }
        }

        protected override String GetConfigFileLocation()
        {
            return AppConfigurationCallback.GetFileName();
        }

        protected void PopulateFromConfigFile()
        {
            var configLocation = GetConfigFileLocation();
            if (configLocation == null || configLocation.Trim() == "")
            {
                throw new Exception("Unable to find the Config location");
            }

            var configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = configLocation };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            base.PopulateFromConfigFile(config, configLocation);

            IndexBaseFolder = config.AppSettings.Settings["IndexBaseFolder"] != null ? config.AppSettings.Settings["IndexBaseFolder"].Value : LogLocation;
            IndexBaseFolder = LogLocation.Replace("|ConfigPath|", FileUtils.GetFileDirectory(configLocation));
            IndexBaseFolder = Path.GetFullPath((new Uri(IndexBaseFolder)).LocalPath);

            ServerPort = config.AppSettings.Settings["ServerPort"] != null ? SafeUtils.Int(config.AppSettings.Settings["ServerPort"].Value) : ServerPort;
            AppName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        }
    }
}
