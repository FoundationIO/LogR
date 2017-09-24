using System.ServiceProcess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;

namespace LogR.Web
{
    public static class LoggerWindowsServiceExtensions
    {
        public static void RunAsService(this IWebHost host)
        {
            var webHostService = new LoggerWindowsService(host);
            ServiceBase.Run(webHostService);
        }
    }
}