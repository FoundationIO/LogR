using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data.Migrations;
using LogR.Common.Interfaces.Repository;
using LogR.Common.Interfaces.Service.Config;
using LogR.Repository.Migration;

namespace LogR.Service.Migration
{
    public class MigrationService : IMigrationService
    {
        private IDBMigration migration;
        private IAppConfiguration config;
        private ISqlBasedIndexStoreDBMigration sqlIndexStoreMigration;

        public MigrationService(IDBMigration migration , IAppConfiguration config , ISqlBasedIndexStoreDBMigration sqlIndexStoreMigration)
        {
            this.migration = migration;
            this.config = config;
            this.sqlIndexStoreMigration = sqlIndexStoreMigration;
        }

        public void MigrateSqlBasedIndexStore()
        {
            if (config.IsSqlBasedIndexStore())
            {
                if (sqlIndexStoreMigration.IsMigrationUptoDate() == false)
                {
                    if (sqlIndexStoreMigration.MigrateToLatestVersion() == false)
                        throw new Exception("Unable to update the Database version");
                }
            }
        }

        public void MigrateLocalDatastoreIfNeeded()
        {
            Console.WriteLine("Starting the Migration process");
            if (migration.IsMigrationUptoDate())
            {
                Console.WriteLine("Migration is already upto date. Please press any key to exit");
                Console.ReadKey();
            }
            else
            {
                migration.MigrateToLatestVersion();
                Console.WriteLine("Migration completed. Please press any key to exit");
                Console.ReadKey();
            }
        }

        public void MigrateLocalDatastoreConditionally()
        {
            if (migration.IsMigrationUptoDate() == false)
            {
                if (config.DbSettings.AutomaticMigration == false)
                {
                    throw new Exception("Database version is not upto date.Please run the application with the / migration option and make the database version upto date");
                }

                if (migration.MigrateToLatestVersion() == false)
                    throw new Exception("Unable to update the Database version");
            }
        }
    }
}
