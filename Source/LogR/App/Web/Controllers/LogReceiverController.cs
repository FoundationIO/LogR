using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using Framework.Web.Extensions;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            await Task.Run(() =>
            {
                service.AddToQue(StoredLogType.AppLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
            });
        }

        [Route("/queue/performance-log")]
        public async void QueuePerformanceLog()
        {
            await Task.Run(() =>
            {
                service.AddToQue(StoredLogType.PerfLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
            });
        }

        [Route("/queue/web-log")]
        public async void QueueWebLog()
        {
            await Task.Run(() =>
            {
                service.AddToQue(StoredLogType.WebLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
            });
        }

        [Route("/queue/event-log")]
        public async void QueueEventLog()
        {
            await Task.Run(() =>
            {
                service.AddToQue(StoredLogType.WebLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
            });
        }

        [Route("/add/app-log")]
        public async void AddAppLog()
        {
            await Task.Run(() =>
            {
                service.AddToDb(StoredLogType.AppLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
            });
        }

        [Route("/add/performance-log")]
        public async void AddPerformanceLog()
        {
            await Task.Run(() =>
            {
                service.AddToDb(StoredLogType.PerfLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
            });
        }

        [Route("/add/web-log")]
        public async void AddWebLog()
        {
            await Task.Run(() =>
            {
                service.AddToDb(StoredLogType.WebLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
            });
        }

        [Route("/add/event-log")]
        public async void AddEventLog()
        {
            await Task.Run(() =>
            {
                service.AddToDb(StoredLogType.EventLog, Request.GetRawBodyStringAsync().Result, DateTime.UtcNow, GetApplicationId());
            });
        }

        private int GetApplicationId()
        {
            return 0;
        }
    }
}
