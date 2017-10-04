using Framework.Infrastructure.Models.Config;

namespace Framework.Infrastructure.Config
{
    public interface IBaseConfiguration
    {
        string AppName { get; }

        LogSettings LogSettings { get; }

        DbSettings DbSettings { get; }
    }
}
