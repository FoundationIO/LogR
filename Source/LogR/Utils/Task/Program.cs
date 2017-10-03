using System;
using System.IO;
using Framework.Data.Migrations;
using Framework.Infrastructure.DI;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Interfaces.Service.Task;
using LogR.DI;
using LogR.Repository.Migration;

namespace LogR.Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var migration = DISetup.ServiceProvider.GetService<IMigrationService>();
            var configFileCreator = DISetup.ServiceProvider.GetService<ISampleAppConfigFileCreator>();
            var seedCreator = DISetup.ServiceProvider.GetService<ISeedService>();
            var loadTestGenerator = DISetup.ServiceProvider.GetService<ILoadTestService>();
            var repo = DISetup.ServiceProvider.GetService<ILogRepository>();

            var servicename = args.GetParamValueAsString("/servicename", "LoggerService");

            if (args.IsParamValueAvailable("create_config"))
            {
                System.Console.Out.WriteLine($"Creating sample file at {configFileCreator.GetConfigFileLocation()}");
                configFileCreator.Generate();
                System.Console.Out.WriteLine($"Config file is created.");
            }
            else if (args.IsParamValueAvailable("migrate"))
            {
                migration.MigrateLocalDatastoreIfNeeded();
            }
            else if (args.IsParamValueAvailable("seed"))
            {
                var count = args.GetParamValueAs("seed", 500000);
                seedCreator.GenerateLogs(count);
            }
            else if (args.IsParamValueAvailable("loadtest"))
            {
                loadTestGenerator.Run();
            }
            else if (args.IsParamValueAvailable("delete-all"))
            {
                repo.DeleteAllAppLogs();
                repo.DeleteAllPerformanceLogs();
            }
            else
            {
                ShowUsage();
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("/c for Starting in Console Mode");
            Console.WriteLine("/migrate for Starting in Console Mode");
            Console.WriteLine("/create_config for Creating a config file");
            Console.WriteLine("/seed for creating dummy logs");
            Console.WriteLine("/sendlogs for sending logs to rest api");
            Console.ReadKey();
        }
    }
}
