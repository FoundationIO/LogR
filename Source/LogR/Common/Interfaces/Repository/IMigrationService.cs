namespace LogR.Common.Interfaces.Repository
{
    public interface IMigrationService
    {
        void MigrateLocalDatastoreConditionally();

        void MigrateLocalDatastoreIfNeeded();

        void MigrateSqlBasedIndexStore();
    }
}