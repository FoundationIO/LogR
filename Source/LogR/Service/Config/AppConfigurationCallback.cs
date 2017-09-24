using System.IO;
using Framework.Infrastructure.Utils;

namespace LogR.Service.Config
{
    public delegate string OnGetConfigFileName();

    public class AppConfigurationCallback
    {
        private const string LOGFILENAME = "LoggerServerConfig.json";

        public static string GetFileName()
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

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", "..", "..", "..", "..","Configuration", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            ///////
            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", "..", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", "..", "..", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", "..", "..", "..", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            path = FileUtils.Combine(FileUtils.GetApplicationExeDirectory(), "..", "..", "..", "..", "..", "..", "..", "..", LOGFILENAME);
            if (File.Exists(path))
            {
                return path;
            }

            return string.Empty;
        }
    }
}
