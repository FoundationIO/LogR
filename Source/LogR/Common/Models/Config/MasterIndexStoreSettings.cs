using System;
using Framework.Infrastructure.Models.Config;
using Microsoft.Extensions.Configuration;

namespace LogR.Common.Models.Config
{
    public class MasterIndexStoreSettings : BaseSettings
    {
        public MasterIndexStoreSettings(IConfiguration configuration, Func<string,string> configUpdater = null)
            : base(configuration, configUpdater)
        {
        }

        public string MasterIndexServer { get; internal set; }

        public int NumberOfThreads { get; internal set; }

        public int BatchSizeToProcess { get; internal set; }
    }
}
