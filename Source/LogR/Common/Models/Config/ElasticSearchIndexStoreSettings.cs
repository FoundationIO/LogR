using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LogR.Common.Models.Config
{
    public class ElasticSearchIndexStoreSettings : BaseSettings
    {
        public ElasticSearchIndexStoreSettings(IConfiguration configuration)
            : base(configuration)
        {
        }

        public string ServerName { get; internal set; }

        public int PortNumber { get; internal set; }
    }
}
