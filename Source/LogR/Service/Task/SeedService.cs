using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataGenerator;
using DataGenerator.Sources;
using Framework.Infrastructure.Constants;
using Framework.Infrastructure.Utils;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;
using LogR.Common.Interfaces.Service.Task;
using LogR.Common.Models.Logs;

namespace LogR.Service.Task
{
    public class SeedService : ISeedService
    {
        private ILogRepository logRepository;

        static SeedService()
        {
            Generator.Default.Configure(c => c
                .Entity<AppLog>(e =>
                {
                    e.Property(p => p.AppLogId).DataSource<GuidSource>();
                    e.Property(p => p.LogType).DataSource<IntegerSource>();
                    e.Property(p => p.CorelationId).DataSource<GuidSource>();
                    e.Property(p => p.Longdate).DataSource<DateTimeSource>();
                    e.Property(p => p.LongdateAsTicks).Value((p) => p.Longdate.Ticks);
                    e.Property(p => p.Severity).DataSource(new[]
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
                    });
                    e.Property(p => p.App).DataSource(new[]
                    {
                        "MailR",
                        "LogR",
                        "ConfigR",
                        "PocoGenerator",
                        "DataGenerator"
                    });
                    e.Property(p => p.MachineName).DataSource(new[]
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
                    });
                    e.Property(p => p.ProcessId).DataSource<IntegerSource>();
                    e.Property(p => p.ThreadId).DataSource<IntegerSource>();
                    e.Property(p => p.CurrentTag).DataSource<IntegerSource>();
                    e.Property(p => p.CurrentFunction).DataSource<IntegerSource>();
                    e.Property(p => p.CurrentSourceFilename).DataSource<IntegerSource>();
                    e.Property(p => p.CurrentSourceLineNumber).DataSource<IntegerSource>();
                    e.Property(p => p.UserIdentity).DataSource<IntegerSource>();
                    e.Property(p => p.RemoteAddress).DataSource<IntegerSource>();
                    e.Property(p => p.UserAgent).DataSource(new[]
                    {
                        "",
                        "Mozilla 1.0",
                        "Firefox 1.0",
                        "IE 1.0",
                        "IE 11.0",
                        "Netscape 2.0",
                        "Safari 1.0",
                    });
                    e.Property(p => p.Result).DataSource<IntegerSource>();
                    e.Property(p => p.ResultCode).DataSource(new[]
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
                    });
                    e.Property(p => p.Message).DataSource<LoremIpsumSource>();
                    e.Property(p => p.PerfModule).DataSource(new[]
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
                    });
                    e.Property(p => p.PerfFunctionName).DataSource(new[]
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
                    });
                    e.Property(p => p.StartTime).DataSource<DateTimeSource>();
                    e.Property(p => p.ElapsedTime).Value((p) => (p.StartTime.AddMilliseconds(RandomGenerator.Current.Next(100000)) - p.StartTime).TotalMilliseconds);
                    e.Property(p => p.PerfStatus).DataSource(new[]
                    {
                        "",
                        "ERROR",
                        "COMPLETED"
                    });

                    e.Property(p => p.Request).DataSource<IntegerSource>();
                    e.Property(p => p.Response).DataSource<IntegerSource>();
                }));
        }

        public SeedService(ILogRepository logRepository)
        {
            this.logRepository = logRepository;
        }

        public List<AppLog> GetAppLogs(int numberOfLogs)
        {
            return Generator.Default.List<AppLog>(numberOfLogs).ToList();
        }

        public void GenerateLogs(int numberOfLogs)
        {
            Parallel.For(0, numberOfLogs / 20, item =>
            {
                var lst = new List<RawLogData>();
                Generator.Default.List<AppLog>(20).ForEach(appItem =>
                {
                    lst.Add(new RawLogData() { Type = LogType.AppLog, Data = JsonUtils.Serialize(appItem), ReceiveDate = DateTime.UtcNow });
                });
                logRepository.SaveLog(lst);
            });
        }
    }
}
