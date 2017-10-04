using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Framework.Infrastructure.Models.Config
{
    public class DbSettings : BaseSettings
    {
        public DbSettings(
            IConfiguration configuration,
            Func<string, string> configUpdator = null)
            : base(configuration, configUpdator)
        {
        }

        //database related
        public string DatabaseType { get; private set; }

        public string DatabaseName { get; private set; }

        public string DatabaseServer { get; private set; }

        public string DatabaseUserName { get; private set; }

        public string DatabasePassword { get; private set; }

        public bool DatabaseUseIntegratedLogin { get; private set; }

        public int DatabaseCommandTimeout { get; private set; }

        public int MaxPoolSize { get; private set; } = 200;

        public List<KeyValuePair<string, Microsoft.Extensions.Logging.LogLevel>> OtherFrameworkLogSettings { get; private set; }

        //Migration related
        public bool AutomaticMigration { get; set; }

        public string MigrationNamespace { get; set; } = null;

        public string MigrationProfile { get; set; } = null;
    }
}
