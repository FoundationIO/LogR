using Framework.Infrastructure.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Logging
{
    public class FrameworkLoggerProvider : ILoggerProvider
    {
        FrameworkLogger appLogger;

        public FrameworkLoggerProvider(IBaseConfiguration config, ILog log)
        {
            appLogger = new FrameworkLogger(config, log);
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
