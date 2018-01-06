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
        static Regex defaultLogExpression = new Regex("(?<longdatetime>[^\t]*)\t(?<loglevel>[^\t]*)\t(?<machine>[^\t]*)\t(?<processid>[^\t]*)\t(?<threadid>[^\t]*)\t(?<function>[^\t]*)\t(?<filename>[^\t]*)\t(?<linenumber>[^\t]*)\t(?<tag>[^\t]*)\t(?<user>[^\t]*)\t(?<ip>[^\t]*)\t(?<result>[^\t]*)\t(?<elapsedtime>[^\t]*)\t(?<message>[^\t]*)$");
        
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
                    Console.WriteLine("/logpath is not specified (folder path)");
                    return;
                }
                if (String.IsNullOrEmpty(filePattern))
                {
                    Console.WriteLine("/logfilter is not specified (file filter eg. *.log)");
                    return;
                }

                if (String.IsNullOrEmpty(logExtractionPattern))
                {
                    Console.WriteLine("/loglinepattern is not specified, using the default pattern");
                    logExtractionPattern = defaultLogExpression.ToString();
                }
                logFilePathAndPattern.Add(new LogParseInfo { FilePattern = filePattern, LogExtractionPattern = logExtractionPattern , LogFilePath = logFilePath });

                ProcessLogs(true, logFilePathAndPattern);
            }
            else
            {
                if(appSettings.LogFilePathAndPattern != null)
                    logFilePathAndPattern = appSettings.LogFilePathAndPattern;
                Thread t = new Thread(() => ProcessLogs(false, logFilePathAndPattern));
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

        static void ProcessLogs(bool isCommandline, List<LogParseInfo> logFilePathAndPatterns)
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

                    var appLog = GetAppLogFromLine(fileName, line, extractionPattern);
                    if (appLog != null)
                        SendAppLogToServer(appLog);
                }
                SaveFilePosition(fileName, reader.Position);
            }
        }

        static AppLog GetAppLogFromLine(string fileName, string line, string extractionPattern)
        {
            AppLog log = new AppLog();
            Match match;
            if (String.IsNullOrEmpty(extractionPattern))
            {
                match = defaultLogExpression.Match(line);
            }
            else
            {
                Regex expression = new Regex(extractionPattern);
                match = expression.Match(line);
            }

            if (match.Success)
            {
                if (match.Groups["longdatetime"].Value.IsTrimmedStringNotNullOrEmpty())
                {
                    log.Longdate = SafeUtils.DateTime(match.Groups["longdatetime"].Value);
                }
                else if (match.Groups["shorttime"].Value.IsTrimmedStringNotNullOrEmpty())
                {
                    log.Longdate = SafeUtils.DateTime(match.Groups["shorttime"].Value);
                }
                else
                {
                    log.Longdate = DateTime.UtcNow;
                }

                if (match.Groups["loglevel"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.Severity = match.Groups["loglevel"].Value;

                if (match.Groups["machine"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.MachineName = match.Groups["machine"].Value;

                if (SafeUtils.Int(match.Groups["processid"].Value) != 0)
                    log.ProcessId = SafeUtils.Int(match.Groups["processid"].Value);

                if (SafeUtils.Int(match.Groups["threadid"].Value) != 0)
                    log.ThreadId = SafeUtils.Int(match.Groups["threadid"].Value);

                if (match.Groups["function"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.CurrentFunction = match.Groups["function"].Value;

                if (match.Groups["filename"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.CurrentSourceFilename = match.Groups["filename"].Value;

                if (SafeUtils.Int(match.Groups["linenumber"].Value) != 0)
                    log.CurrentSourceLineNumber = SafeUtils.Int(match.Groups["linenumber"].Value);

                if (match.Groups["tag"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.CurrentTag = match.Groups["tag"].Value;

                if (match.Groups["user"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.UserIdentity = match.Groups["user"].Value;

                if (match.Groups["ip"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.RemoteAddress = match.Groups["ip"].Value;

                if (match.Groups["result"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.Result = match.Groups["result"].Value;

                if (match.Groups["loglevel"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.ElapsedTime = SafeUtils.Double(match.Groups["elapsedtime"].Value);

                if (match.Groups["message"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.Message = match.Groups["message"].Value;

                if (match.Groups["log-id"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.LogId = SafeUtils.Guid(match.Groups["log-id"].Value);

                if (match.Groups["log-type"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.LogType = SafeUtils.Int(match.Groups["LogType"].Value);

                if (match.Groups["application-Id"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.ApplicationId = match.Groups["application-Id"].Value;

                if (match.Groups["corelation-id"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.CorelationId = match.Groups["corelation-id"].Value;

                if (match.Groups["receivedDateAsTicks"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.ReceivedDateAsTicks = SafeUtils.Long(match.Groups["receivedDateAsTicks"].Value);

                if (match.Groups["longdate-as-ticks"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.LongdateAsTicks = SafeUtils.Long(match.Groups["longdate-as-ticks"].Value);

                if (match.Groups["app"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.App = match.Groups["app"].Value;

                if (match.Groups["perf-module"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.PerfModule = match.Groups["perf-module"].Value;

                if (match.Groups["result-code"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.ResultCode = SafeUtils.Int(match.Groups["result-code"].Value);

                if (match.Groups["perf-function-name"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.PerfFunctionName = match.Groups["perf-function-name"].Value;

                if (match.Groups["start-time"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.StartTime = SafeUtils.DateTime(match.Groups["start-time"].Value);

                if (match.Groups["perf-status"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.PerfStatus = match.Groups["perf-status"].Value;

                if (match.Groups["request"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.Request = match.Groups["request"].Value;      
                
                if (match.Groups["response"].Value.IsTrimmedStringNotNullOrEmpty())
                    log.Response = match.Groups["response"].Value;
            }
            return log;
        }

        static void SendAppLogToServer(AppLog appLog)
        {
            var serverUrl = "http://localhost:9090";
            FlurlHttp.Configure(settings => settings.OnErrorAsync = HandleFlurlErrorAsync);

            var entry = new RawLogData() { Type = StoredLogType.AppLog, Data = JsonUtils.Serialize(appLog), ReceiveDate = DateTime.UtcNow };

            var result = (serverUrl + ControllerConstants.AddAppLogUrl.AddFirstChar('/'))
                .WithHeader(HeaderContants.AppId, "APPID_1")
                .PostJsonAsync(entry).Result;
        }

        static async Task HandleFlurlErrorAsync(HttpCall call)
        {
            //log.Error("Unable to send log to server - status code = " + call.HttpStatus);
            call.ExceptionHandled = true;
            await System.Threading.Tasks.Task.Run(() => { Thread.Sleep(1); });
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
