namespace Framework.Data.Migrations
{
    public interface IDBMigration
    {
        bool IsMigrationUptoDate();

        bool MigrateToLatestVersion();
    }
}