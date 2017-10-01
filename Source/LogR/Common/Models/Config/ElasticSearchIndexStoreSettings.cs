using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Models.Config;
using Microsoft.Extensions.Configuration;

namespace LogR.Common.Models.Config
{
    public class ElasticSearchIndexStoreSettings : BaseSettings
    {
        public ElasticSearchIndexStoreSettings(IConfiguration configuration, Func<string, string> configUpdater = null)
            : base(configuration, configUpdater)
        {
        }

        public string ServerName { get; internal set; }

        public int PortNumber { get; internal set; }

        public string AppLogIndex { get; internal set; }

        public string PerformanceLogIndex { get; internal set; }
    }
}
