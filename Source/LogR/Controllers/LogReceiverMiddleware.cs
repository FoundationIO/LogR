using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using LogR.Code.Service;
using Framework.Contants;
using System.Text;

namespace LogR.Controllers
{
    public class LogReceiverMiddleware
    {
        ILogCollectService service;
        public LogReceiverMiddleware(ILogCollectService service, RequestDelegate next)
        {
            _next = next;
            this.service = service;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/add/app-log"))
            {                
                service.AddToQue(LogType.AppLog, ReadBody(context), DateTime.UtcNow);
                await context.Response.WriteAsync("OK");
            }
            else if (context.Request.Path.StartsWithSegments("/add/performance-log"))
            {
                service.AddToQue(LogType.PerformanceLog, ReadBody(context), DateTime.UtcNow);
                await context.Response.WriteAsync("OK");
            }
            else
            {
                await _next(context);
            }
        }
        readonly RequestDelegate _next;

        public string ReadBody(HttpContext context)
        {
            var initialBody = context.Request.Body; // Workaround
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

    }

    public static class LogReceiverMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogReceiverMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogReceiverMiddleware>();
        }
    }
}
