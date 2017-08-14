using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LogR.Common.Interfaces.Service.Config;
using Microsoft.Extensions.Configuration;

namespace LogR.Service.Config
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

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(configLocation))
                .AddJsonFile(Path.GetFileName(configLocation), optional: false, reloadOnChange: true);

            var config = builder.Build();

            var appSettings = config.GetSection("configuration:appSettings");
            
            base.PopulateFromConfigFile(appSettings, configLocation);

            if (MigrationNamespace.IsTrimmedStringNullOrEmpty())
            {
                throw new Exception("MigrationNamespace is empty");
            }

            IndexBaseFolder = appSettings["indexBaseFolder"] != null ? appSettings["indexBaseFolder"] : IndexBaseFolder;
            if (IndexBaseFolder != null && IndexBaseFolder.Contains("|ConfigPath|"))
            {
                IndexBaseFolder = IndexBaseFolder.Replace("|ConfigPath|", FileUtils.GetFileDirectory(configLocation));
                IndexBaseFolder = Path.GetFullPath((new Uri(IndexBaseFolder)).LocalPath);
            }

            ServerPort = SafeUtils.Int(appSettings["serverPort"], ServerPort);
            AppName = Path.GetFileNameWithoutExtension(this.GetType().GetTypeInfo().Assembly.Location);
        }
    }
}
