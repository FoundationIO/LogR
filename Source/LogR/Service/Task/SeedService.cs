using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using Bogus.Extensions;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Utils;
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
        private ILogWriteRepository logWriteRepository;

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
                .RuleFor(p => p.Message, f => f.Lorem.Sentence())
                .RuleFor(p => p.PerfModule, f => f.PickRandom(new[]
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
                .RuleFor(p => p.PerfFunctionName, f => f.PickRandom(new[]
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
                .RuleFor(p => p.PerfStatus, f => f.PickRandom(new[]
                        {
                            "",
                            "ERROR",
                            "COMPLETED"
                        }))
                .RuleFor(p => p.Request, f => f.Lorem.Sentence())
                .RuleFor(p => p.Response, f => f.Lorem.Sentence());
        }

        public SeedService(ILogWriteRepository logWriteRepository)
        {
            this.logWriteRepository = logWriteRepository;
        }

        public List<AppLog> GetAppLogs(int numberOfLogs)
        {
            return fakeAppLogs.Generate(numberOfLogs).ToList();
        }

        public void GenerateLogs(int numberOfLogs)
        {
            Parallel.For(0, numberOfLogs / 20, item =>
            {
                var lst = new List<RawLogData>();
                GetAppLogs(20).ForEach(appItem =>
                {
                    lst.Add(new RawLogData() { Type = StoredLogType.AppLog, Data = JsonUtils.Serialize(appItem), ReceiveDate = DateTime.UtcNow });
                });
                logWriteRepository.SaveLog(lst);
            });
        }
    }
}
