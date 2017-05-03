using Framework.Contants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure
{
    public interface IBaseConfiguration
    {
        bool LogTraceEnable { get; }
        bool LogDebugEnable { get; }
        bool LogInfoEnable { get; }
        bool LogSqlEnable { get; }
        bool LogWarnEnable { get; }
        String LogLocation { get; }
        string DatabaseType { get; }
        string DatabaseName { get; }
        string DatabaseServer { get; }
        string DatabaseUserName { get; }
        string DatabasePassword { get;}
        int DatabaseCommandTimeout { get; }
        string AppName { get; set; }
        string MigrationNamespace { get; set; }

    }
}
