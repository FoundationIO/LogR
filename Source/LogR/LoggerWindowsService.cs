using System.ComponentModel;
using System.Threading;
using System;
using System.ServiceProcess;
using Framework.Utils;
using LogR.Code.Infrastructure;
using Framework.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;

namespace LogR
{
    internal class LoggerWindowsService : WebHostService
    {
        public LoggerWindowsService(IWebHost host) : base(host)
        {
        }

        protected override void OnStarting(string[] args)
        {
            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            base.OnStopping();
        }
    }

    public static class LoggerWindowsServiceExtensions
    {
        public static void RunAsService(this IWebHost host)
        {
            var webHostService = new LoggerWindowsService(host);
            ServiceBase.Run(webHostService);
        }
    }
}