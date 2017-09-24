using System;
using System.Linq;
using System.Reflection;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using Framework.Data.DbAccess;
using Framework.Infrastructure.Config;
using Framework.Infrastructure.Logging;

namespace Framework.Data.Migrations
{
    public class DBMigration : IDBMigration
    {
        private ILog log;
        private IDBInfo dbInfo;
        private string migrationNamespace;

        public DBMigration(IBaseConfiguration config , ILog log, IDBInfo dbInfo)
        {
            this.log = log;
            this.dbInfo = dbInfo;
            migrationNamespace = config.MigrationNamespace;
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

        private Assembly GetMigrationAssembly(string migrationNamespace)
        {
            var qry = from a in AppDomain.CurrentDomain.GetAssemblies()
                      from t in a.GetTypes()
                      where t.Namespace != null ? t.Namespace.ToLower().Equals(migrationNamespace.ToLower().Trim()) : false
                      select a;
            return qry.FirstOrDefault();
        }
    }
}
