using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Framework.Infrastructure.Utils;

namespace LogR.Task
{
    class Program
    {
        static BlockingCollection<string> modifiedFileCollection = new BlockingCollection<string>();
        static void Main(string[] args)
        {
            var logFilePath = args.GetParamValueAsString("logpath");
            var filePattern = args.GetParamValueAsString("logfilter", "*.log");
            var logExtractionPattern = args.GetParamValueAsString("loglinepattern");

            if (args.IsParamAvailable("c"))
            {
                NewThread(true, logFilePath, filePattern);
            }
            else
            {
                Thread t = new Thread(() => NewThread(false,logFilePath,filePattern));
                t.IsBackground = true;
                t.Start();
            }
        }

        static long GetLastReadPositionForFile(string fileName)
        {
            return 0;
        }

        static void NewThread(bool isCommandline, string logFilePath, string filePattern = "*.log")
        {
            var fileWatcher = new FileSystemWatcher
            {
                Path = logFilePath,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = filePattern,
                EnableRaisingEvents = true,
                
            };

            fileWatcher.Changed += (object sender, FileSystemEventArgs e) =>
            {
                modifiedFileCollection.Add(e.FullPath);
            };

            do
            {
                var fileName = modifiedFileCollection.Take();
                DoParseAndSendLogs(fileName, GetLastReadPositionForFile(fileName));
                Thread.Sleep(100);
            } while (true); 
        }


        static void DoParseAndSendLogs(string fileName, long lastLeftPosition)
        {
            
        }
    }
}
