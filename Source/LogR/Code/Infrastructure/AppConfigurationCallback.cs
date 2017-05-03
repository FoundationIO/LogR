using Framework.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.Code.Infrastructure
{
    public class AppConfigurationCallback
    {
        public delegate string OnGetConfigFileName();
        public static OnGetConfigFileName GetFileName = OnConfigurationGetFilename;

        private const string LOGFILENAME = "LoggerServerConfig.config";

        public static string OnConfigurationGetFilename()
        {
            var path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "Configuration", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "Configuration", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "Configuration", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", "Configuration", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }
            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", "..", "Configuration", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }
            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", "..", ".." ,"Configuration", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }
            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", "..", "..", "..", "Configuration", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }
            return "";
        }

        public static string OnCreateConfigGetFilename()
        {
            return FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", ".." , "Configuration", LOGFILENAME);
        }
    }
}
