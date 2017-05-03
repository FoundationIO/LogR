using Framework.Contants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogR.Code.Models.Logs
{
    public class RawLogData
    {
        public LogType Type { get; set; }
        public string Data { get; set; }
        public DateTime ReceiveDate { get; set; }
    }
}

