using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Framework.Infrastructure.Logging;
using LogR.Common.Interfaces.Service.App;
using LogR.Common.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace LogR.Web.Identity
{
    public class LogRUserStore : GenericLogRUserStore<LogRIdentityUser>
    {
        public LogRUserStore(ILog log, IAccountService context)
            : base(log, context)
        {
        }
    }
}
