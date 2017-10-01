using System.Collections.Concurrent;
using System.Linq;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Models.Config;
using Microsoft.Extensions.Logging;

namespace Framework.Infrastructure.Logging
{
    public class FrameworkLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, FrameworkLogger> loggers = new ConcurrentDictionary<string, FrameworkLogger>();
        private IBaseConfiguration config;
        private ILog log;

        public FrameworkLoggerProvider(IBaseConfiguration config, ILog log)
        {
            this.config = config;
            this.log = log;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
        }

        private FrameworkLogger CreateLoggerImplementation(string name)
        {
            var settings = GetLogSettingForSection(name);
            return new FrameworkLogger(settings, log);
        }

        private LogSettings GetLogSettingForSection(string name)
        {
            if (config.LogSettings.OtherFrameworkLogSettings == null
                || config.LogSettings.OtherFrameworkLogSettings.Count == 0
                || config.LogSettings.OtherFrameworkLogSettings.Count(x => name.ToLower().Trim().StartsWith(x.Key.ToLower().Trim())) == 0)
            {
                if (config.LogSettings.OtherFrameworkLogSettings.Exists(x => x.Key.ToLower().Trim() == "Default".ToLower()))
                {
                    var defaultItem = config.LogSettings.OtherFrameworkLogSettings.FirstOrDefault(x => x.Key.ToLower().Trim() == "Default".ToLower());
                    return GetSettingsForLogLevel(defaultItem.Value);
                }
                else
                {
                    return config.LogSettings.NoOpLogSettings();
                }
            }

            var entry = config.LogSettings.OtherFrameworkLogSettings.FirstOrDefault(x => name.ToLower().Trim().StartsWith(x.Key.ToLower().Trim()));
            return GetSettingsForLogLevel(entry.Value);
        }

        private LogSettings GetSettingsForLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return config.LogSettings.TraceLogSettings();
                case LogLevel.Information:
                    return config.LogSettings.InfoLogSettings();

                case LogLevel.Warning:
                    return config.LogSettings.WarnLogSettings();

                case LogLevel.Critical:
                case LogLevel.Error:
                    return config.LogSettings.ErrorLogSettings();

                default:
                case LogLevel.None:
                    return config.LogSettings.NoOpLogSettings();
            }
        }

        private LogSettings GetDefaultOrNoOpSettings()
        {
            // fixme: need to change this
            return config.LogSettings;
        }
    }
}
