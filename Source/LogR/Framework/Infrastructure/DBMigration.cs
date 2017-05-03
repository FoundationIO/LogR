using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using System.Reflection;
using FluentMigrator.Runner.Initialization;
using LogR.Code.Infrastructure;

namespace Framework.Infrastructure
{
    public class DBMigration : IDBMigration
    {
        ILog log;
        IDBInfo dbInfo;
        string migrationNamespace;

        public DBMigration(IAppConfiguration config , ILog log, IDBInfo dbInfo)
        {
            this.log = log;
            this.dbInfo = dbInfo;
            migrationNamespace = config.MigrationNamespace;
        }

        public class MigrationOptions : IMigrationProcessorOptions
        {
            public bool PreviewOnly { get; set; }
            public string ProviderSwitches { get; set; }
            public int Timeout { get; set; }
        }

        


        public bool IsMigrationUptoDate()
        {
            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer)
            {
                Namespace = migrationNamespace
            };

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory = dbInfo.GetMigrationProcessorFactory();
            using (var processor = factory.Create(dbInfo.GetConnectionString(), announcer, options))
            {
                var runner = new MigrationRunner(assembly, migrationContext, processor);
                if (runner.MigrationLoader.LoadMigrations()
                    .Any(pair => !runner.VersionLoader.VersionInfo.HasAppliedMigration(pair.Key)))
                {
                    return false;
                }
            }
            return true;
        }

        public bool MigrateToLatestVersion()
        {
            var announcer = new TextWriterAnnouncer(s => log.Info(s));
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer)
            {
                Namespace = migrationNamespace
            };

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory = dbInfo.GetMigrationProcessorFactory();
            using (var processor = factory.Create(dbInfo.GetConnectionString(), announcer, options))
            {
                var runner = new MigrationRunner(assembly, migrationContext, processor);
                runner.MigrateUp(true);
            }

            return true;
        }
    }
}
