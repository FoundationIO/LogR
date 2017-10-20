using System;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using LogR.Common.Interfaces.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using LogR.Common.Enums;

namespace LogR.Web.Controllers
{
    public class LogReceiverMiddleware
    {
        private readonly RequestDelegate _next;

        private ILogCollectService service;

        public LogReceiverMiddleware(ILogCollectService service, RequestDelegate next)
        {
            _next = next;
            this.service = service;
        }

        int GetApplicationId()
        {
            return 0;
        }

        public async Task Invoke(HttpContext context)
        {
            int applicationId = GetApplicationId();

            if (context.Request.Path.StartsWithSegments("/queue/app-log"))
            {
                service.AddToQue(StoredLogType.AppLog, ReadBody(context), DateTime.UtcNow, applicationId);
                await context.Response.WriteAsync("OK");
            }
            else if (context.Request.Path.StartsWithSegments("/queue/performance-log"))
            {
                service.AddToQue(StoredLogType.PerfLog, ReadBody(context), DateTime.UtcNow, applicationId);
                await context.Response.WriteAsync("OK");
            }
            else if (context.Request.Path.StartsWithSegments("/add/app-log"))
            {
                service.AddToDb(StoredLogType.AppLog, ReadBody(context), DateTime.UtcNow, applicationId);
                await context.Response.WriteAsync("OK");
            }
            else if (context.Request.Path.StartsWithSegments("/add/performance-log"))
            {
                service.AddToDb(StoredLogType.PerfLog, ReadBody(context), DateTime.UtcNow, applicationId);
                await context.Response.WriteAsync("OK");
            }
            else
            {
                await _next(context);
            }
        }

        public string ReadBody(HttpContext context)
        {
            var initialBody = context.Request.Body;
            var buffer = new byte[Convert.ToInt64(context.Request.ContentLength)];
            context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
