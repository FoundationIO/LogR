using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogR.Common.Interfaces.Service.Config;

namespace LogR.Service.Config
{
    public class SampleAppConfigFileCreator : ISampleAppConfigFileCreator
    {
        public void Generate()
        {
            var fileName = GetConfigFileLocation();
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            File.WriteAllText(fileName, GetDefaultConfigContent());
        }

        public string GetConfigFileLocation()
        {
            var fileName = Directory.GetCurrentDirectory() + "\\..\\..\\..\\..\\Configuration\\" + "LoggerServerConfig.config";
            return fileName;
        }

        private string GetDefaultConfigContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>\n<configuration>\n<appSettings>\n ");
            sb.AppendLine("<add key=\"IndexBaseFolder\" value=\"|ConfigPath|\\..\\Data\\Index\\\" /> <!-- Folder where Log data is stored -->");
            sb.AppendLine("<add key=\"LogTraceEnable\" value=\"true\" />");
            sb.AppendLine("<add key=\"LogDebugEnable\" value=\"true\" />");
            sb.AppendLine("<add key=\"LogInfoEnable\" value=\"true\" />");
            sb.AppendLine("<add key=\"LogSqlEnable\" value=\"true\" />");
            sb.AppendLine("<add key=\"LogWarnEnable\" value=\"true\" />");
            sb.AppendLine("<add key=\"LogLocation\" value=\"|ConfigPath|\\..\\Data\\Logs\\\" />");
            sb.AppendLine("<add key=\"ServerPort\" value=\"8080\" /> ");
            sb.AppendLine("<add key=\"DatabaseType\" value=\"SQLITE3\" /> <!-- SQLITE3, MYSQL, MSSQL2008 -->");
            sb.AppendLine("<add key=\"DatabaseName\" value=\"|ConfigPath|\\..\\Data\\Database\\loggerInfo.sqlite3\" />");
            sb.AppendLine("<add key=\"DatabaseServer\" value=\"\" /> <!-- Required for Types MYSQL, MSSQL2008 -->");
            sb.AppendLine("<add key=\"DatabaseUserName\" value=\"\" /> <!-- Required for Types MYSQL, MSSQL2008 -->");
            sb.AppendLine("<add key=\"DatabasePassword\" value=\"\" /> <!-- Required for Types MYSQL, MSSQL2008 -->");
            sb.AppendLine("<add key=\"DatabaseCommandTimeout\" value=\"100\" />");
            sb.AppendLine(" \n</appSettings>\n</configuration>");
            return sb.ToString();
        }
    }

}
