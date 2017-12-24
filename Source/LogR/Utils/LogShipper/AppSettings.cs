using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.LogShipper
{
    public class AppSettings
    {
        public string LogServerUrl { get; set; }
        public List<LogParseInfo> LogFilePathAndPattern { get; set; } = new List<LogParseInfo>();
    }
}
