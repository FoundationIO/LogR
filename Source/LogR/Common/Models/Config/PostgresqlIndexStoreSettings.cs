using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Common.Models.Config
{
    public class PostgresqlIndexStoreSettings
    {
        public string DbLocation { get; internal set; }

        public string DatabaseName { get; internal set; }

        public string DatabaseServer { get; internal set; }

        public string DatabaseUserName { get; internal set; }

        public string DatabasePassword { get; internal set; }

        public int DatabaseCommandTimeout { get; internal set; }

        public bool DatabaseUseIntegratedLogin { get; internal set; }
    }
}
