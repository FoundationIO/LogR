using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LogR.Common.Models.Config
{
    public class EmbeddedElasticSearchIndexStoreSettings : BaseSettings
    {
        public EmbeddedElasticSearchIndexStoreSettings(IConfiguration configuration)
            : base(configuration)
        {
        }

        public string IndexBaseFolder { get; internal set; }
    }
}
