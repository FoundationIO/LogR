using Framework.Infrastructure.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Logging
{
    public static class AppLoggerFactoryExtensions
    {
        public static ILoggerFactory AddAppLogger(this ILoggerFactory loggerFactory, IBaseConfiguration config, ILog log)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            loggerFactory.AddProvider(new AppLoggerProvider(config, log));
            return loggerFactory;
        }

    }
}
