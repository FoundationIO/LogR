using System;
using Framework.Infrastructure.Constants;
using LogR.Common.Enums;

namespace LogR.Common.Models.Logs
{
    public class RawLogData
    {
        public StoredLogType Type { get; set; }

        public string Data { get; set; }

        public bool IsListData { get; set; }

        public DateTime ReceiveDate { get; set; }

        public string ApplicationId { get; set; }
    }
}
