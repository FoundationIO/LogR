using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Models.Config;
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
        LogSettings LogSettings { get; }
        string DatabaseType { get; }
        string DatabaseName { get; }
        string DatabaseServer { get; }
        string DatabaseUserName { get; }
        string DatabasePassword { get;}
        bool DatabaseUseIntegratedLogin { get; }
        int DatabaseCommandTimeout { get; }
        string AppName { get; set; }
        string MigrationNamespace { get; set; }

    }
}
