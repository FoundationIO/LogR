using System;
using System.IO;
using Framework.Data.Migrations;
using Framework.Infrastructure.DI;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;
using LogR.DI;
using LogR.Repository.Migration;
using Microsoft.AspNetCore.Hosting;

namespace LogR.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = DISetup.ServiceProvider.GetService<IAppConfiguration>();
            var migration = DISetup.ServiceProvider.GetService<IMigrationService>();

            migration.MigrateSqlBasedIndexStore();
            migration.MigrateLocalDatastoreConditionally();

            var hostBuilder = new WebHostBuilder().UseKestrel();

            if (config.ServerPort > 0)
            {
                hostBuilder = hostBuilder.UseUrls($"http://0.0.0.0:{config.ServerPort}");
            }

            hostBuilder = hostBuilder.UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();

            var host = hostBuilder.Build();

            host.Run();
        }
    }
}
