using Framework.Contants;
using Framework.Infrastructure;
using Framework.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Code.Infrastructure
{
    public interface IAppConfiguration : IBaseConfiguration
    {
        int ServerPort { get;}

        String IndexBaseFolder { get; }

        String AppLogIndexFolder { get; }

        String PerformanceLogIndexFolder { get; }
    }
}
