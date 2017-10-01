using System;
using Framework.Infrastructure.Models.Config;
using Microsoft.Extensions.Configuration;

namespace LogR.Common.Models.Config
{
    public class SqlBasedIndexStoreSettings : BaseSettings
    {
        public SqlBasedIndexStoreSettings(IConfiguration configuration, Func<string,string> configUpdater = null)
            : base(configuration, configUpdater)
        {
        }

        public string DbLocation { get; internal set; }

        public string DatabaseName { get; internal set; }

        public string DatabaseServer { get; internal set; }

        public string DatabaseUserName { get; internal set; }

        public string DatabasePassword { get; internal set; }

        public int DatabaseCommandTimeout { get; internal set; }

        public bool DatabaseUseIntegratedLogin { get; internal set; }
    }
}
