namespace Framework.Infrastructure
{
    public interface IDBMigration
    {
        bool IsMigrationUptoDate();
        bool MigrateToLatestVersion();
    }
}