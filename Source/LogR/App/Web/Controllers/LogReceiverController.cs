using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using Framework.Web.Extensions;
using LogR.Common.Constants;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LogR.Web.Controllers
{
    public class LogReceiverController : Controller
    {
        private ILogCollectService service;

        public LogReceiverController(ILogCollectService service)
        {
            this.service = service;
        }

        [Route(ControllerConstants.QueueAppLogUrl)]
        public void QueueAppLog()
        {
            service.AddListToQue(StoredLogType.AppLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
        }

        [Route(ControllerConstants.QueueAppLogListUrl)]
        public void QueueAppLogList()
        {
            service.AddToQue(StoredLogType.AppLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
        }

        [Route(ControllerConstants.QueuePerformanceLogUrl)]
        public void QueuePerformanceLog()
        {
            service.AddToQue(StoredLogType.PerfLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
        }

        [Route(ControllerConstants.QueueWebLogUrl)]
        public void QueueWebLog()
        {
            service.AddToQue(StoredLogType.WebLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
        }

        [Route(ControllerConstants.QueueEventLogUrl)]
        public void QueueEventLog()
        {
            service.AddToQue(StoredLogType.EventLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
        }

        [Route(ControllerConstants.AddAppLogUrl)]
        public void AddAppLog()
        {
            service.AddToDb(StoredLogType.AppLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
        }

        [Route(ControllerConstants.AddPerformanceLogUrl)]
        public void AddPerformanceLog()
        {
            service.AddToDb(StoredLogType.PerfLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
        }

        [Route(ControllerConstants.AddWebLogUrl)]
        public void AddWebLog()
        {
            service.AddToDb(StoredLogType.WebLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
        }

        [Route(ControllerConstants.AddEventLogUrl)]
        public void AddEventLog()
        {
            service.AddToDb(StoredLogType.EventLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
        }

        private string GetApplicationId()
        {
            if (this.HttpContext.Request.Headers.ContainsKey(HeaderContants.AppId) == false)
                return null;
            return this.HttpContext.Request.Headers[HeaderContants.AppId];
        }
    }
}
