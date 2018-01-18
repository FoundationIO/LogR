using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogR.LogShipper
{
    public class LogParseInfo
    {
        public string LogFilePath { get; set; }
        public string FilePattern { get; set; }
        public string LogExtractionPattern { get; set; }
        public string ExtractionPatternForFileName { get; set; }
    }
}
