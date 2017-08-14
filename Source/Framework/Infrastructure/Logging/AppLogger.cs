
using Framework.Infrastructure.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Logging
{
    public class AppLogger : ILogger
    {
        IBaseConfiguration config;
        ILog log;
        public AppLogger(IBaseConfiguration config, ILog log)
        {
            this.config = config;
            this.log = log;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message))
                return;

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    log.Debug(exception, message);
                    break;

                case LogLevel.Information:
                    log.Info(exception, message);
                    break;

                case LogLevel.Warning:
                    log.Warn(exception, message);
                    break;

                case LogLevel.Error:
                    log.Error(exception, message);
                    break;

                case LogLevel.Critical:
                    log.Fatal(exception, message);
                    break;

                case LogLevel.None:
                    break;
                default:
                    log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                    log.Info(exception, message);
                    break;
            }

        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return config.LogDebug;
                case LogLevel.Information:
                    return config.LogInfo;
                case LogLevel.Warning:
                    return config.LogWarn;
                case LogLevel.Error:
                    return config.LogError;
                case LogLevel.Critical:
                    return true;
                case LogLevel.None:
                    return false;
                default:
                    log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                    return false;
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
