using Framework.Infrastructure.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Logging
{
    public class AppLoggerProvider : ILoggerProvider
    {
        AppLogger appLogger;

        public AppLoggerProvider(IBaseConfiguration config, ILog log)
        {
            appLogger = new AppLogger(config, log);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return appLogger;
        }

        public void Dispose()
        {

        }

    }
}
