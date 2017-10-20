using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogR.Common.Models.Identity
{
    public class LogRUserLoginInfo
    {
        public LogRUserLoginInfo(string loginProvider, string providerKey, string displayName)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            ProviderDisplayName = displayName;
        }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public string ProviderDisplayName { get; set; }
    }
}
