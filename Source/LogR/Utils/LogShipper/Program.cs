using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Framework.Infrastructure.Utils;
using LogR.Common.Constants;
using LogR.Common.Enums;
using LogR.Common.Models.Logs;

namespace LogR.LogShipper
{
    class Program
    {
        static BlockingCollection<string> modifiedFileCollection = new BlockingCollection<string>();
        static List<FileParserInfo> fileParserInfoStateList = new List<FileParserInfo>();
        //static ILog log;
        static Regex logExpression = new Regex("(?<longdatetime>[^\t]*)\t(?<loglevel>[^\t]*)\t(?<machine>[^\t]*)\t(?<processid>[^\t]*)\t(?<threadid>[^\t]*)\t(?<function>[^\t]*)\t(?<filename>[^\t]*)\t(?<linenumber>[^\t]*)\t(?<tag>[^\t]*)\t(?<user>[^\t]*)\t(?<ip>[^\t]*)\t(?<result>[^\t]*)\t(?<elapsedtime>[^\t]*)\t(?<message>[^\t]*)$");
        
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
                    //return;
                }
                logFilePathAndPattern.Add(new LogParseInfo { FilePattern = filePattern, LogExtractionPattern = logExtractionPattern , LogFilePath = logFilePath });

                NewThread(true, logFilePathAndPattern);
            }
            else
            {
                if(appSettings.LogFilePathAndPattern != null)
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
            var filename = GetAppSettingsFile();
            if (File.Exists(filename))
            {
                var result = new AppSettings();
                var data = File.ReadAllText(GetAppSettingsFile());
                if (string.IsNullOrEmpty(data))
                    return result;
                result = JsonUtils.Deserialize<AppSettings>(data);
                return result;
            }
            else
                return null;
        }

        static List<FileParserInfo> GetFileParserInfoStateData()
        {
            var result = new List<FileParserInfo>();
            var fileName = GetFileParserInfoStateFile();
            if (File.Exists(fileName))
            {
                var data = File.ReadAllText(GetFileParserInfoStateFile());
                if (string.IsNullOrEmpty(data))
                    return result;

                result = JsonUtils.Deserialize<List<FileParserInfo>>(data);

                return result;
            }
            return null;           
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
                    DoParseAndSendLogs(fileName, GetLastReadPositionForFile(fileName), null);
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
                    DoParseAndSendLogs(file, GetLastReadPositionForFile(file), item.LogExtractionPattern);
                }                
            }
        }


        static void DoParseAndSendLogs(string fileName, long lastLeftPosition, string extractionPattern)
        {
            using (var reader = new PositionableStreamReader(fileName,Encoding.UTF8))
            {
                reader.Position = lastLeftPosition;
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var appLog = GetAppLogFromLine(line, extractionPattern);
                    if (appLog != null)
                        SendAppLogToServer(appLog);
                }
                SaveFilePosition(fileName, reader.Position);
            }
        }

        static AppLog GetAppLogFromLine(string line, string extractionPattern)
        {
            AppLog log = new AppLog();
            Match match;
            if (String.IsNullOrEmpty(extractionPattern))
            {
                match = logExpression.Match(line);
            }
            else
            {
                Regex expression = new Regex(extractionPattern);
                match = expression.Match(line);
            }

            if (match.Success)
            {
                if(SafeUtils.DateTime(match.Groups["longdatetime"].Value) != null)
                    log.Longdate = Convert.ToDateTime(match.Groups["longdatetime"].Value); 

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["loglevel"].Value))
                    log.Severity = match.Groups["loglevel"].Value;

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["machine"].Value))
                    log.MachineName = match.Groups["machine"].Value;

                if (SafeUtils.Int(match.Groups["processid"].Value) != 0)
                    log.ProcessId = SafeUtils.Int(match.Groups["processid"].Value);

                if (SafeUtils.Int(match.Groups["threadid"].Value) != 0)
                    log.ThreadId = SafeUtils.Int(match.Groups["threadid"].Value);

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["function"].Value))
                    log.CurrentFunction = match.Groups["function"].Value;

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["filename"].Value))
                    log.CurrentSourceFilename = match.Groups["filename"].Value;

                if (SafeUtils.Int(match.Groups["linenumber"].Value) != 0)
                    log.CurrentSourceLineNumber = SafeUtils.Int(match.Groups["linenumber"].Value);

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["tag"].Value))
                    log.CurrentTag = match.Groups["tag"].Value;

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["user"].Value))
                    log.UserIdentity = match.Groups["user"].Value;

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["ip"].Value))
                    log.RemoteAddress = match.Groups["ip"].Value;

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["result"].Value))
                    log.Result = match.Groups["result"].Value;

                if (SafeUtils.Double(match.Groups["loglevel"].Value) != 0)
                    log.ElapsedTime = SafeUtils.Double(match.Groups["elapsedtime"].Value);

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["message"].Value))
                    log.Message = match.Groups["message"].Value;

                if (SafeUtils.Guid(match.Groups["log-id"].Value) != Guid.Empty)
                    log.LogId = SafeUtils.Guid(match.Groups["log-id"].Value);

                if (SafeUtils.Int(match.Groups["log-type"].Value) != 0)
                    log.LogType = SafeUtils.Int(match.Groups["LogType"].Value);

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["application-Id"].Value))
                    log.ApplicationId = match.Groups["application-Id"].Value;

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["corelation-id"].Value))
                    log.CorelationId = match.Groups["corelation-id"].Value;

                if (SafeUtils.Long(match.Groups["receivedDateAsTicks"].Value) != 0)
                    log.ReceivedDateAsTicks = SafeUtils.Long(match.Groups["receivedDateAsTicks"].Value);

                if (SafeUtils.Long(match.Groups["longdate-as-ticks"].Value) != 0)
                    log.LongdateAsTicks = SafeUtils.Long(match.Groups["longdate-as-ticks"].Value);

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["app"].Value))
                    log.App = match.Groups["app"].Value;

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["perf-module"].Value))
                    log.PerfModule = match.Groups["perf-module"].Value;

                if (SafeUtils.Int(match.Groups["result-code"].Value) != 0)
                    log.ResultCode = SafeUtils.Int(match.Groups["result-code"].Value);

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["perf-function-name"].Value))
                    log.PerfFunctionName = match.Groups["perf-function-name"].Value;

                if (SafeUtils.DateTime(match.Groups["start-time"].Value) != null)
                    log.StartTime = SafeUtils.DateTime(match.Groups["start-time"].Value);

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["perf-status"].Value))
                    log.PerfStatus = match.Groups["perf-status"].Value;

                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["request"].Value))
                    log.Request = match.Groups["request"].Value;      
                
                if (StringUtils.IsTrimmedStringNotNullOrEmpty(match.Groups["response"].Value))
                    log.Response = match.Groups["response"].Value;
            }
            return log;
        }

        static void SendAppLogToServer(AppLog appLog)
        {
            var serverUrl = "http://localhost:9090";
            FlurlHttp.Configure(settings => settings.OnErrorAsync = HandleFlurlErrorAsync);

            GenerateLogsInternal(appLog, actionToSend: null, actionToAdd: (entry) =>
            {
                var result = (serverUrl + ControllerConstants.AddAppLogUrl.AddFirstChar('/'))
                    .WithHeader(HeaderContants.AppId, "APPID_1")
                    .PostJsonAsync(entry).Result;
            });
        }        

        static async Task HandleFlurlErrorAsync(HttpCall call)
        {
            //log.Error("Unable to send log to server - status code = " + call.HttpStatus);
            call.ExceptionHandled = true;
            await System.Threading.Tasks.Task.Run(() => { Thread.Sleep(1); });
        }

        static List<AppLog> GetAppLogs(int numberOfLogs)
        {
            return new List<AppLog>();
        }

        static void GenerateLogsInternal(AppLog appLog, Action<RawLogData> actionToAdd, Action<RawLogData> actionToSend)
        {                  
            if(appLog != null)
            {
                var entry = new RawLogData() { Type = StoredLogType.AppLog, Data = JsonUtils.Serialize(appLog), ReceiveDate = DateTime.UtcNow };

                if (actionToAdd != null)
                    actionToAdd(entry);

                if (actionToSend != null)
                {
                    actionToSend(entry);
                }
            }
        }

        static void SaveFilePosition(string fileName, long position)
        {
            if (fileParserInfoStateList == null)
            {
                fileParserInfoStateList = new List<FileParserInfo>();
            }
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
