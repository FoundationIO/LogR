using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Models.Config;
using Microsoft.Extensions.Configuration;

namespace LogR.Common.Models.Config
{
    public class EmbeddedElasticSearchIndexStoreSettings : BaseSettings
    {
        public EmbeddedElasticSearchIndexStoreSettings(IConfiguration configuration, Func<string, string> configUpdater = null)
            : base(configuration, configUpdater)
        {
        }

        public string IndexBaseFolder { get; internal set; }
    }
}
