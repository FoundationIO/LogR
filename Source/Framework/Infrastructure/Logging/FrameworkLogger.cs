using System;
using Framework.Infrastructure.Models.Config;
using Microsoft.Extensions.Logging;

namespace Framework.Infrastructure.Logging
{
    public class FrameworkLogger : ILogger
    {
        private LogSettings logConfig;
        private ILog log;

        public FrameworkLogger(LogSettings logConfig, ILog log)
        {
            this.logConfig = logConfig;
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
                    return logConfig.LogDebug;
                case LogLevel.Information:
                    return logConfig.LogInfo;
                case LogLevel.Warning:
                    return logConfig.LogWarn;
                case LogLevel.Error:
                    return logConfig.LogError;
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
