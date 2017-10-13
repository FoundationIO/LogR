using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Models.Config;
using Framework.Infrastructure.Utils;
using LogR.Common.Constants;
using Microsoft.Extensions.Configuration;

namespace LogR.Common.Models.Config
{
    public class LuceneIndexStoreSettings : BaseSettings
    {
        public LuceneIndexStoreSettings(IConfiguration configuration, Func<string, string> configUpdater = null)
            : base(configuration, configUpdater)
        {
        }

        public string IndexBaseFolder { get; internal set; }

        public string AppLogIndexFolder
        {
            get
            {
                return IndexBaseFolder + "\\" + StringConstants.Config.AppLogIndex + "\\";
            }
        }

        public string PerformanceLogIndexFolder
        {
            get
            {
                return IndexBaseFolder + "\\" + StringConstants.Config.PerformanceLogIndex + "\\";
            }
        }

        public string WebLogIndexFolder
        {
            get
            {
                return IndexBaseFolder + "\\" + StringConstants.Config.WebLogIndex + "\\";
            }
        }

        public string EventLogIndexFolder
        {
            get
            {
                return IndexBaseFolder + "\\" + StringConstants.Config.EventLogIndex + "\\";
            }
        }
    }
}
