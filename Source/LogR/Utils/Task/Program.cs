using System;
using System.IO;
using Framework.Data.Migrations;
using Framework.Infrastructure.DI;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.Log;
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

            var servicename = args.GetParamValueAsString("/servicename", "LoggerService");

            if (args.IsParamValueAvailable("create_config"))
            {
                System.Console.Out.WriteLine($"Creating sample file at {configFileCreator.GetConfigFileLocation()}");
                configFileCreator.Generate();
                System.Console.Out.WriteLine($"Config file is created.");
            }
            else if (args.IsParamValueAvailable("migrate"))
            {
                System.Console.Out.WriteLine($"Running the APP Database Migration");
                migration.MigrateLocalDatastoreIfNeeded();
                System.Console.Out.WriteLine($"Migration is completed.");
            }
            else if (args.IsParamValueAvailable("send-logs"))
            {
                System.Console.Out.WriteLine($"Sending Log Generation to remote Log server... sending 500000 log entries... please wait...");
                var count = args.GetParamValueAs("log-count", 500000);
                seedCreator.SendLogsToRemote(count,"http://localhost:9090");
                System.Console.Out.WriteLine($"Sample Logs are sent.");
            }
            else if (args.IsParamValueAvailable("load-test"))
            {
                System.Console.Out.WriteLine($"Running Load test...");
                loadTestGenerator.Run();
                System.Console.Out.WriteLine($"Load Test is completed.");
            }
            else if (args.IsParamValueAvailable("delete-all-logs"))
            {
                System.Console.Out.WriteLine($"Deleting all logs...");
                var repo = DISetup.ServiceProvider.GetService<ILogWriteRepository>();
                repo.DeleteAllLogs();
                System.Console.Out.WriteLine($"All Logs are deleted.");
            }
            else
            {
                ShowUsage();
            }
        }

        private static void ShowUsage()
        {
            Console.WriteLine("/c\t\t\tfor Starting in Console Mode");
            Console.WriteLine("/migrate\t\t\tfor Starting in Console Mode");
            Console.WriteLine("/create-config\t\t\tfor Creating a config file");
            Console.WriteLine("/send-logs\t\t\tfor sending logs to rest api");
            Console.WriteLine("/delete-all-logs\t\t\tfor sending logs to rest api");
            Console.ReadKey();
        }
    }
}
