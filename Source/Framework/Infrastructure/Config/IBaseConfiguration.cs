using Framework.Infrastructure.Models.Config;

namespace Framework.Infrastructure.Config
{
    public interface IBaseConfiguration
    {
        string AppName { get; }

        //Migration related
        bool AutomaticMigration { get; }

        string MigrationNamespace { get; }

        string MigrationProfile { get; }

        LogSettings LogSettings { get; }

        DbSettings DbSettings { get; }
    }
}
