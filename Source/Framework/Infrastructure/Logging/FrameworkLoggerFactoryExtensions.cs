using System;
using Framework.Infrastructure.Config;
using Microsoft.Extensions.Logging;

namespace Framework.Infrastructure.Logging
{
    public static class FrameworkLoggerFactoryExtensions
    {
        public static ILoggerFactory AddFrameworkLogger(this ILoggerFactory loggerFactory, IBaseConfiguration config, ILog log)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            loggerFactory.AddProvider(new FrameworkLoggerProvider(config, log));
            return loggerFactory;
        }
    }
}
