using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using Bogus.Extensions;
using Flurl;
using Flurl.Http;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Logging;
using Framework.Infrastructure.Utils;
using LogR.Common.Constants;
using LogR.Common.Enums;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Repository.Log;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Interfaces.Service.Task;
using LogR.Common.Models.Logs;

namespace LogR.Service.Task
{
    public class SeedService : ISeedService
    {
        private static Faker<AppLog> fakeAppLogs;
        private ILog log;

        static SeedService()
        {
            Randomizer.Seed = new Random(3897235);

            fakeAppLogs = new Faker<AppLog>()
                .RuleFor(u => u.LogId, f => f.Random.Uuid())
                .RuleFor(p => p.LogType, f => f.Random.Number(0,4))
                .RuleFor(p => p.CorelationId, f => Guid.NewGuid().ToString())
                .RuleFor(p => p.FunctionId, f => Guid.NewGuid().ToString())
                .RuleFor(p => p.Longdate, f => f.Date.Past())
                .RuleFor(p => p.LongdateAsTicks, (f,u) => u.Longdate.Ticks)
                .RuleFor(p => p.Severity, f => f.PickRandom(new[]
                    {
                        Strings.Log.Error,
                        Strings.Log.Trace,
                        Strings.Log.Error,
                        Strings.Log.Warning,
                        Strings.Log.Info,
                        Strings.Log.Fatal,
                        Strings.Log.Debug,
                        Strings.Log.SqlBeginTransaction,
                        Strings.Log.SqlCommitTransaction,
                        Strings.Log.SqlRollbackTransaction,
                        Strings.Log.Sql,
                        Strings.Log.SqlError,
                    }))
                .RuleFor(p => p.App, f => f.PickRandom(new[]
                    {
                        "MailR",
                        "LogR",
                        "ConfigR",
                        "PocoGenerator",
                        "DataGenerator"
                    }))
                .RuleFor(p => p.MachineName, f => f.PickRandom(new[]
                    {
                        "PROD-WEB-01",
                        "PROD-WEB-02",
                        "PROD-WEB-03",
                        "PROD-WEB-01",
                        "PROD-WEB-02",
                        "PROD-WEB-03",
                        "PROD-WEB-01",
                        "PROD-WEB-02",
                        "PROD-WEB-03",
                        "PROD-WEB-01",
                        "PROD-WEB-02",
                        "PROD-WEB-03",
                        "PROD-WEB-01",
                        "PROD-WEB-02",
                        "PROD-WEB-03",
                        "PROD-QUE-01",
                        "PROD-QUE-02",
                        "PROD-QUE-03",
                        "PROD-SCH-01",
                        "PROD-SCH-02",
                        "PROD-SCH-03",
                        "PROD-IDX-01",
                        "PROD-IDX-02",
                        "PROD-IDX-03",
                        "PROD-DB-01",
                        "PROD-DB-02",
                        "PROD-DB-03",
                    }))
                .RuleFor(p => p.ProcessId, f => f.Random.Number(1,50000))
                .RuleFor(p => p.ThreadId, f => f.Random.Number(1,300))
                .RuleFor(p => p.CurrentFunction, f => f.PickRandom(new[]
                    {
                        "Trace",
                        "Debug",
                        "BuildIndex",
                        "DoSearch",
                        "SaveToFile",
                        "LoadFromFile",
                        "UpdateSystemSettings",
                        "HandleException",
                        "Index",
                        "Index",
                        "FindById",
                        "UploadFile",
                        "DeleteFile",
                        "OptimizeIndex",
                        "CreateIndex",
                    }))
                .RuleFor(p => p.CurrentSourceFilename, f => f.System.FileName())
                .RuleFor(p => p.CurrentSourceLineNumber, f => f.Random.Number(1,1000))
                .RuleFor(p => p.UserIdentity, (f) => f.Internet.UserName())
                .RuleFor(p => p.RemoteAddress, f => f.Internet.Ip())
                .RuleFor(p => p.UserAgent, f => f.Internet.UserAgent())
                .RuleFor(p => p.Result, f => "")
                .RuleFor(p => p.ResultCode, f => f.PickRandom(new[]
                    {
                        200,
                        200,
                        200,
                        200,
                        200,
                        200,
                        200,
                        200,
                        200,
                        200,
                        500,
                        500,
                        500,
                        500,
                        401,
                        401,
                        404,
                        404,
                        404
                    }))
                .RuleFor(p => p.Message, f => f.Lorem.Sentence(300))
                .RuleFor(p => p.Module, f => f.PickRandom(new[]
                    {
                        "Log",
                        "Indexer",
                        "Search",
                        "Configuration",
                        "SqliteDataSource",
                        "PostgresqlDataSource",
                        "LuceneDataSource",
                        "RaptorDBDataSource",
                        "WebUI",
                        "APILayer",
                    }))
                .RuleFor(p => p.FunctionName, f => f.PickRandom(new[]
                    {
                        "Trace",
                        "Debug",
                        "BuildIndex",
                        "DoSearch",
                        "SaveToFile",
                        "LoadFromFile",
                        "UpdateSystemSettings",
                        "HandleException",
                        "Index",
                        "Index",
                        "FindById",
                        "UploadFile",
                        "DeleteFile",
                        "OptimizeIndex",
                        "CreateIndex",
                    }))
                .RuleFor(p => p.StartTime, f => f.Date.Past())
                .RuleFor(p => p.ElapsedTime, (f,p) => (p.StartTime.AddMilliseconds(f.Random.Number(1,100000)) - p.StartTime).TotalMilliseconds) //).Value()
                .RuleFor(p => p.Result, f => f.PickRandom(new[]
                        {
                            "",
                            "ERROR",
                            "COMPLETED"
                        }))
                .RuleFor(p => p.Request, f => f.Lorem.Sentence())
                .RuleFor(p => p.Response, f => f.Lorem.Sentence());
        }

        public SeedService(ILog log)
        {
            this.log = log;
        }

        public List<AppLog> GetAppLogs(int numberOfLogs)
        {
            return fakeAppLogs.Generate(numberOfLogs).ToList();
        }

        public void SendLogsToRemote(int numberOfLogs, string serverUrl)
        {
            FlurlHttp.Configure(settings => settings.OnErrorAsync = HandleFlurlErrorAsync);

            GenerateLogsInternal(numberOfLogs, actionToSend: null, actionToAdd: (entry) =>
            {
                var result = (serverUrl + ControllerConstants.QueueAppLogUrl.AddFirstChar('/'))
                    .WithHeader(HeaderContants.AppId, "APPID_1")
                    .PostJsonAsync(entry).Result;
            });
        }

        private async System.Threading.Tasks.Task HandleFlurlErrorAsync(HttpCall call)
        {
            log.Error("Unable to send log to server - status code = " + call.HttpStatus);
            call.ExceptionHandled = true;
            await System.Threading.Tasks.Task.Run(() => { Thread.Sleep(1); });
        }

        private void GenerateLogsInternal(int numberOfLogs, Action<RawLogData> actionToAdd, Action<List<RawLogData>> actionToSend)
        {
            var logsToCreate = 20;
            var numberOfThreads = numberOfLogs / 20;
            if (numberOfLogs < logsToCreate)
            {
                logsToCreate = numberOfLogs;
                numberOfThreads = 1;
            }

            Parallel.For(0, numberOfThreads, item =>
            {
                log.Error($"Sending batch - {item} of {numberOfLogs / 20}");
                var lst = new List<RawLogData>();
                GetAppLogs(logsToCreate).ForEach(appItem =>
                {
                    var entry = new RawLogData() { Type = StoredLogType.AppLog, Data = JsonUtils.Serialize(appItem), ReceiveDate = DateTime.UtcNow };
                    if (actionToAdd != null)
                        actionToAdd(entry);
                    lst.Add(entry);
                });

                if (actionToSend != null)
                {
                    actionToSend(lst);
                }
            });
        }
    }
}
