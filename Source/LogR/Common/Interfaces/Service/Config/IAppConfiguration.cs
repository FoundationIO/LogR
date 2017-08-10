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

namespace LogR.Common.Interfaces.Service.Config
{
    public interface IAppConfiguration : IBaseConfiguration
    {
        int ServerPort { get;}

        String IndexBaseFolder { get; }

        String AppLogIndexFolder { get; }

        String PerformanceLogIndexFolder { get; }
    }
}
