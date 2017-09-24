using System;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure.Constants;
using LogR.Common.Interfaces.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace LogR.Web.Controllers
{
    public static class LogReceiverMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogReceiverMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogReceiverMiddleware>();
        }
    }
}
