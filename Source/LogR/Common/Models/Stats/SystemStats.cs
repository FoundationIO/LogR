using System;

namespace LogR.Common.Models.Stats
{
    public class SystemStats
    {
        public static long StaticLastNumberOfLogsDeleted { get; set; } = 0;

        public static DateTime StaticLastLogDeletedDateTime { get; set; } = DateTime.Now;

        public long LastNumberOfLogsDeleted
        {
            get
            {
                return StaticLastNumberOfLogsDeleted;
            }
        }

        public DateTime LastLogDeletedDateTime
        {
            get
            {
                return StaticLastLogDeletedDateTime;
            }
        }

        public ulong AppDataFolderSize { get; set; }

        public ulong PerformanceDataFolderSize { get; set; }

        public ulong LogFolderSize { get; set; }

        public long LogFileCount { get; set; }
    }
}
