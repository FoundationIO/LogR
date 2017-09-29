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
                    e.Property(p => p.LongdateAsTicks).DataSource<IntegerSource>();
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
                    e.Property(p => p.App).DataSource<LastNameSource>();
                    e.Property(p => p.MachineName).DataSource<LastNameSource>();
                    e.Property(p => p.ProcessId).DataSource<IntegerSource>();
                    e.Property(p => p.ThreadId).DataSource<IntegerSource>();
                    e.Property(p => p.CurrentTag).DataSource<IntegerSource>();
                    e.Property(p => p.CurrentFunction).DataSource<IntegerSource>();
                    e.Property(p => p.CurrentSourceFilename).DataSource<IntegerSource>();
                    e.Property(p => p.CurrentSourceLineNumber).DataSource<IntegerSource>();
                    e.Property(p => p.UserIdentity).DataSource<IntegerSource>();
                    e.Property(p => p.RemoteAddress).DataSource<IntegerSource>();
                    e.Property(p => p.UserAgent).DataSource<IntegerSource>();
                    e.Property(p => p.Result).DataSource<IntegerSource>();
                    e.Property(p => p.ResultCode).DataSource<IntegerSource>();
                    e.Property(p => p.Message).DataSource<IntegerSource>();
                    e.Property(p => p.PerfModule).DataSource<IntegerSource>();
                    e.Property(p => p.PerfFunctionName).DataSource<IntegerSource>();
                    e.Property(p => p.StartTime).DataSource<IntegerSource>();
                    e.Property(p => p.ElapsedTime).DataSource<IntegerSource>();
                    e.Property(p => p.PerfStatus).DataSource<IntegerSource>();
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
            var logs = Generator.Default.List<AppLog>(numberOfLogs);
            foreach (var item in logs)
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    logRepository.SaveLog(new RawLogData() { Type = LogType.AppLog, Data = JsonUtils.Serialize(item), ReceiveDate = DateTime.UtcNow });
                });
            }
        }
    }
}
