using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        }

        //General App Related
        public string AppName { get; set; } = null;

        public DbSettings DbSettings { get; private set; }

        // log related
        public LogSettings LogSettings { get; private set; }

        protected void PrepareFolders()
        {
            if (Directory.Exists(LogSettings.LogLocation) == false)
            {
                Directory.CreateDirectory(LogSettings.LogLocation);
            }

            if (DbSettings.DatabaseType == DBType.SQLITE3)
            {
                if (Directory.Exists(FileUtils.GetFileDirectory(DbSettings.DatabaseName)) == false)
                    Directory.CreateDirectory(FileUtils.GetFileDirectory(DbSettings.DatabaseName));
            }
        }

        protected virtual string GetConfigFileLocation()
        {
            return string.Empty;
        }

        protected void PopulateFromConfigFile(IConfigurationSection appSettings, IConfigurationSection frameworkLogSetting, string configLocation)
        {
            //Load .Net core's Logging configuration
            var otherLogSettings = new List<KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>>();
            var logLevelSettings = frameworkLogSetting.GetSection(Strings.Config.LogLevel);
            foreach (var setting in logLevelSettings.GetChildren())
            {
                var logLevel = SafeUtils.Enum<Microsoft.Extensions.Logging.LogLevel>(setting.Value, Microsoft.Extensions.Logging.LogLevel.None);
                otherLogSettings.Add(new KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>(setting.Key, logLevel));
            }

            var logSection = appSettings.GetSection(Strings.Config.LogSettings);
            var ls = logSection.GetChildren().Select(x => x.Key);
            LogSettings = new LogSettings(logSection, otherLogSettings, (str) =>
            {
                if (str.IsTrimmedStringNotNullOrEmpty() && str.Contains(Strings.Config.ConfigPath))
                {
                    str = str.Replace(Strings.Config.ConfigPath, FileUtils.GetFileDirectory(configLocation));
                    str = Path.GetFullPath(new Uri(str).LocalPath);
                }
                return str;
            });

            DbSettings = new DbSettings(appSettings.GetSection(Strings.Config.DbSettings), (str) =>
            {
                if (str.IsTrimmedStringNotNullOrEmpty() && str.Contains(Strings.Config.ConfigPath))
                {
                    str = str.Replace(Strings.Config.ConfigPath, FileUtils.GetFileDirectory(configLocation));
                    str = Path.GetFullPath(new Uri(str).LocalPath);
                }
                return str;
            });
            AppName = Path.GetFileNameWithoutExtension(GetType().GetTypeInfo().Assembly.Location);
        }
    }
}
