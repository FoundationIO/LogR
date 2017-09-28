using System;
using System.IO;
using Framework.Data.Migrations;
using Framework.Infrastructure.DI;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using LogR.Common.Interfaces.Service.Config;
using LogR.DI;
using Microsoft.AspNetCore.Hosting;
using LogR.Repository.Migration;

namespace LogR.Web
{
    public class Program
    {
        //for simplified service check - http://dotnetthoughts.net/how-to-host-your-aspnet-core-in-a-windows-service/

        public static void Main(string[] args)
        {
            var servicename = args.GetParamValueAsString("/servicename", "LoggerService");

            if (args.IsParamValueAvailable("/?") || args.IsParamValueAvailable("/help"))
            {
                Console.WriteLine("/c for Starting in Console Mode");
                Console.WriteLine("/migrate for Starting in Console Mode");
                Console.WriteLine("/create_config for Creating a config file");
                Console.ReadKey();
            }
            else if (args.IsParamValueAvailable("/create_config"))
            {
                var gen = DISetup.ServiceProvider.GetService<ISampleAppConfigFileCreator>();
                System.Console.Out.WriteLine($"Creating sample file at {gen.GetConfigFileLocation()}");
                gen.Generate();
                System.Console.Out.WriteLine($"Config file is created.");
            }
            else
            {
                var migration = DISetup.ServiceProvider.GetService<IDBMigration>();
                var config = DISetup.ServiceProvider.GetService<IAppConfiguration>();

                if (config.IsSqlBasedIndexStore())
                {
                    var sqlIndexStoreMigration = DISetup.ServiceProvider.GetService<ISqlBasedIndexStoreDBMigration>();
                    if (sqlIndexStoreMigration.IsMigrationUptoDate() == false)
                    {
                        if (sqlIndexStoreMigration.MigrateToLatestVersion() == false)
                            throw new Exception("Unable to update the Database version");
                    }                        
                }

                if (args.IsParamValueAvailable("/migrate"))
                {
                    Console.WriteLine("Starting the Migration process");
                    if (migration.IsMigrationUptoDate())
                    {
                        Console.WriteLine("Migration is already upto date. Please press any key to exit");
                        Console.ReadKey();
                    }
                    else
                    {
                        migration.MigrateToLatestVersion();
                        Console.WriteLine("Migration completed. Please press any key to exit");
                        Console.ReadKey();
                    }
                }
                else
                {
                    if (migration.IsMigrationUptoDate() == false)
                    {
                        if (config.AutomaticMigration == false)
                        {
                            throw new Exception("Database version is not upto date.Please run the application with the / migration option and make the database version upto date");
                        }

                        if(migration.MigrateToLatestVersion() == false)
                            throw new Exception("Unable to update the Database version");

                    }

                    var host = new WebHostBuilder()
                        .UseKestrel()
                        .UseUrls($"http://0.0.0.0:{config.ServerPort}")
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup>()
                        .Build();

                    if (args.IsParamValueAvailable("/C") || Environment.UserInteractive)
                    {
                        host.Run();
                    }
                    else
                    {
                        host.RunAsService();
                    }
                }
            }
        }
    }
}
