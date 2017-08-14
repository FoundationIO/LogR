using Framework.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Config
{
    public interface IBaseConfiguration
    {
        bool LogTrace { get; }
        bool LogDebug { get; }
        bool LogInfo { get; }
        bool LogSql { get; }
        bool LogWarn { get; }
        bool LogError { get; }
        bool LogToFile { get; }
        bool LogToDebugger { get; }
        bool LogToConsole { get; }
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
