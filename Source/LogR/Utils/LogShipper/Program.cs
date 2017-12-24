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
using LogR.Common.Models.Logs;

namespace LogR.LogShipper
{
    class Program
    {
        static BlockingCollection<string> modifiedFileCollection = new BlockingCollection<string>();
        static List<FileParserInfo> fileParserInfoStateList = new List<FileParserInfo>();

        static void Main(string[] args)
        {
            fileParserInfoStateList = GetFileParserInfoStateData();
            var appSettings = GetAppSettings();

            var logFilePathAndPattern = new List<LogParseInfo>();

            if (args.IsParamAvailable("c"))
            {
                var logFilePath = args.GetParamValueAsString("logpath");
                var filePattern = args.GetParamValueAsString("logfilter", "*.log");
                var logExtractionPattern = args.GetParamValueAsString("loglinepattern");
                if (String.IsNullOrEmpty(logFilePath))
                {
                    Console.WriteLine("logpath is not specified");
                    return;
                }
                if (String.IsNullOrEmpty(filePattern))
                {
                    Console.WriteLine("/logfilter is not specified");
                    return;
                }
                if (String.IsNullOrEmpty(logExtractionPattern))
                {
                    Console.WriteLine("/loglinepattern is not specified");
                    return;
                }
                logFilePathAndPattern.Add(new LogParseInfo { FilePattern = filePattern, LogExtractionPattern = logExtractionPattern , LogFilePath = logFilePath });

                NewThread(true, logFilePathAndPattern);
            }
            else
            {
                logFilePathAndPattern = appSettings.LogFilePathAndPattern;
                Thread t = new Thread(() => NewThread(false, logFilePathAndPattern));
                t.IsBackground = true;
                t.Start();
            }
        }

        static long GetLastReadPositionForFile(string fileName)
        {
            return 0;
        }

        static AppSettings GetAppSettings()
        {
            var result = new AppSettings();
            var data = File.ReadAllText(GetAppSettingsFile());
            if (string.IsNullOrEmpty(data))
                return result;
            result = JsonUtils.Deserialize<AppSettings>(data);
            return result;
        }

        static List<FileParserInfo> GetFileParserInfoStateData()
        {
            var result = new List<FileParserInfo>();
            var data = File.ReadAllText(GetFileParserInfoStateFile());
            if (string.IsNullOrEmpty(data))
                return result;

            result = JsonUtils.Deserialize<List<FileParserInfo>>(data);

            return result;
        }

        static void NewThread(bool isCommandline, List<LogParseInfo> logFilePathAndPatterns)
        {
            if(!isCommandline)
            {
                foreach (var pattern in logFilePathAndPatterns)
                {
                    var fileWatcher = new FileSystemWatcher
                    {
                        Path = pattern.LogFilePath,
                        NotifyFilter = NotifyFilters.LastWrite,
                        Filter = pattern.FilePattern,
                        EnableRaisingEvents = true,
                    };

                    fileWatcher.Changed += (object sender, FileSystemEventArgs e) =>
                    {
                        modifiedFileCollection.Add(e.FullPath);
                    };
                }
                do
                {
                    var fileName = modifiedFileCollection.Take();
                    DoParseAndSendLogs(fileName, GetLastReadPositionForFile(fileName));
                    Thread.Sleep(100);
                } while (true);
            }
            else
            {
                var item = logFilePathAndPatterns.FirstOrDefault();
                if (item == null)
                {
                    Console.WriteLine("You need to pass atleast one file name");
                }
                foreach(var file in Directory.GetFiles(item.LogFilePath,item.FilePattern))
                {
                    DoParseAndSendLogs(file, GetLastReadPositionForFile(file));
                }                
            }
        }


        static void DoParseAndSendLogs(string fileName, long lastLeftPosition)
        {
            using (var reader = new PositionableStreamReader(fileName,Encoding.UTF8))
            {
                reader.Position = lastLeftPosition;
                while(reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var appLog = GetAppLogFromLine(line);
                    if (appLog != null)
                        SendAppLogToServer(appLog);
                }
                SaveFilePosition(fileName, reader.Position);
            }
        }

        static AppLog GetAppLogFromLine(string line)
        {
            return new AppLog();
        }

        static void SendAppLogToServer(AppLog appLog)
        {
            //send the log to server
        }

        static void SaveFilePosition(string fileName, long position)
        {
            var fInfo = fileParserInfoStateList.FirstOrDefault(x => x.FileName.ToUpper() == fileName.ToUpper());
            if (fInfo == null)
                fInfo = new FileParserInfo { FileName = fileName};
            
            fInfo.LastReadPosition = position;
            fInfo.LastUpdatedTime = DateTime.Now;

            File.WriteAllText(GetFileParserInfoStateFile(), JsonUtils.Serialize(fInfo));
        }

        static string GetFileParserInfoStateFile()
        {
            return Path.Combine(Directory.GetCurrentDirectory(),"CurrentFileParserState.json");
        }

        static string GetAppSettingsFile()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "LogFileAndPatternSettings.json");
        }
        
    }
}
