using System;
using System.IO;
using Framework.Infrastructure.DI;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using LogR.DI;
using LogR.Common.Interfaces.Service.Config;
using Framework.Data.Migrations;
using LogR.Repository.Migration;
using LogR.Common.Interfaces.Repository;

namespace LogR.Task
{
    class Program
    {
        static void Main(string[] args)
        {
            var migration = DISetup.ServiceProvider.GetService<IMigrationService>();
            var configFileCreator = DISetup.ServiceProvider.GetService<ISampleAppConfigFileCreator>();

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
                migration.MigrateLocalDatastoreIfNeeded();
            }
            else if (args.IsParamValueAvailable("sendlogs"))
            {
                migration.MigrateLocalDatastoreIfNeeded();
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
