using System;
using System.IO;
using Framework.Infrastructure.DI;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using LogR.DI;

namespace LogR.Task
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicename = args.GetParamValueAsString("/servicename", "LoggerService");

            if (args.IsParamValueAvailable("/?") || args.IsParamValueAvailable("/help"))
            {
                Console.WriteLine("/c for Starting in Console Mode");
                Console.WriteLine("/migrate for Starting in Console Mode");
                Console.WriteLine("/create_config for Creating a config file");
                Console.ReadKey();
            }

        }
    }
}
