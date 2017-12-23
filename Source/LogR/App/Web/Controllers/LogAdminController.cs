using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Infrastructure.Models.Result;
using LogR.Common.Constants;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace LogR.Web.Controllers
{
    public class LogAdminController : Controller
    {
        private ILogUpdateService service;

        public LogAdminController(ILogUpdateService service)
        {
            this.service = service;
        }

        [Route(ControllerConstants.DeleteAllLogsUrl)]
        public ReturnModel<bool> DeleteAllLogs()
        {
            service.DeleteAllLogs((int)StoredLogType.AppLog);
            return service.DeleteAllLogs((int)StoredLogType.PerfLog);
        }
    }
}
