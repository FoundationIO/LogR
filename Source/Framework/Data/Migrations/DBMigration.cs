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
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;
using Framework.Data.DbAccess;

namespace Framework.Data.Migrations
{
    public class DBMigration : IDBMigration
    {
        ILog log;
        IDBInfo dbInfo;
        string migrationNamespace;

        public DBMigration(IBaseConfiguration config , ILog log, IDBInfo dbInfo)
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

        private Assembly GetMigrationAssembly(string migrationNamespace)
        {
            var qry = from a in AppDomain.CurrentDomain.GetAssemblies()
                      from t in a.GetTypes()
                      where (t.Namespace != null ? t.Namespace.ToLower().Equals(migrationNamespace.ToLower().Trim()) : false)
                      select a;
            return qry.FirstOrDefault();
        }

        public bool IsMigrationUptoDate()
        {
            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var assembly = GetMigrationAssembly(migrationNamespace);

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
            var assembly = GetMigrationAssembly(migrationNamespace);

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
