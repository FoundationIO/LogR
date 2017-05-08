using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogR.Common.Models.Stats
{
    public class SystemStats
    {
        public static long LastNumberOfLogsDeleted { get; set; } = 0;
        public static DateTime LastLogDeletedDateTime { get; set; } = DateTime.Now;

        public ulong AppDataFolderSize { get; set; }
        public ulong PerformanceDataFolderSize { get; set; }
        public ulong LogFolderSize { get; set; }
        public long LogFileCount { get; set; }
    }
}
