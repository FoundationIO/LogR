using System;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using LogR.Common.Interfaces.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Framework.Web.Extensions;

namespace LogR.Web.Controllers
{
    public class LogReceiverController : Controller
    {
        private ILogCollectService service;

        public LogReceiverController(ILogCollectService service)
        {
            this.service = service;
        }

        [Route("/queue/app-log")]
        public async void QueueAppLog()
        {
            service.AddToQue(LogType.AppLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow);
        }

        [Route("/queue/performance-log")]
        public async void QueuePerformanceLog()
        {
            service.AddToQue(LogType.PerformanceLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow);
        }

        [Route("/queue/web-log")]
        public async void QueueWebLog()
        {
            service.AddToQue(LogType.WebLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow);
        }

        [Route("/queue/event-log")]
        public async void QueueEventLog()
        {
            service.AddToQue(LogType.WebLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow);
        }

        [Route("/add/app-log")]
        public async void AddAppLog()
        {
            service.AddToDb(LogType.AppLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow);
        }

        [Route("/add/performance-log")]
        public async void AddPerformanceLog()
        {
            service.AddToDb(LogType.PerformanceLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow);
        }

        [Route("/add/web-log")]
        public async void AddWebLog()
        {
            service.AddToDb(LogType.WebLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow);
        }

        [Route("/add/event-log")]
        public async void AddEventLog()
        {
            service.AddToDb(LogType.EventLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow);
        }
    }
}
