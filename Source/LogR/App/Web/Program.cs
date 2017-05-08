using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.ServiceProcess;
using LogR.DI;
using Framework.Data.Migrations;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Utils;
using Framework.Infrastructure.DI;
using LogR.Common.Interfaces.Service.Config;

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
                IDBMigration migration = DISetup.ServiceProvider.GetService<IDBMigration>();
                var log = DISetup.ServiceProvider.GetService<ILog>();
                var config = DISetup.ServiceProvider.GetService<IAppConfiguration>();

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
                        throw new Exception("Database version is not upto date.Please run the application with the / migration option and make the database version upto date");
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
                        //Console.WriteLine("Please press any key to exit");
                        //Console.ReadKey();
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
