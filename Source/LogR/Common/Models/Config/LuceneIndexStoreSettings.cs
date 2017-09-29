using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;

namespace LogR.Common.Models.Config
{
    public class LuceneIndexStoreSettings : BaseSettings
    {
        public LuceneIndexStoreSettings(IConfiguration configuration)
            : base(configuration)
        {
        }

        public string IndexBaseFolder { get; internal set; }
    }
}
