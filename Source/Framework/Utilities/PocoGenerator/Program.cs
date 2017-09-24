using System;
using Framework.Utilities.PocoGenerator.Utilities;

namespace Framework.Utilities.PocoGenerator
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                return -1;
            }

            if (!(args.IsParamValueAvailable("-config") ||
                    args.IsParamValueAvailable("-new") ||
                    (args.IsParamValueAvailable("-connectionString") &&
                     args.IsParamValueAvailable("-dbtype") &&
                     args.IsParamValueAvailable("-templatefile") &&
                     args.IsParamValueAvailable("-codefile"))))
            {
                PrintUsage();
                return -1;
            }

            Config config = new Config(args);
            config.Load();

            DbGenerator generator = new DbGenerator(config);
            generator.LoadAndGenerate();

            return 0;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("\t -new  <config_file.json> : Create new configuration file");
            Console.WriteLine("\t -config <config_file.json>        : Specify all the configuration in a single file ");
            Console.WriteLine("Following used to send data without config file.");
            Console.WriteLine("You need to provide all the following command lines");
            Console.WriteLine("\t -connectionString <connection_string>");
            Console.WriteLine("\t -dbtype <dbType>   : sqlite3, mssql");
            Console.WriteLine("\t -templatefile <template_file.cshtml>   : This is a razor template file");
            Console.WriteLine("\t -codefile <code_file_to_generate.cs>   : This is a output file generated from template file");
        }
    }
}
