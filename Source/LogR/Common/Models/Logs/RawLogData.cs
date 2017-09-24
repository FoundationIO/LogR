using System;
using Framework.Infrastructure.Constants;

namespace LogR.Common.Models.Logs
{
    public class RawLogData
    {
        public LogType Type { get; set; }

        public string Data { get; set; }

        public DateTime ReceiveDate { get; set; }
    }
}
