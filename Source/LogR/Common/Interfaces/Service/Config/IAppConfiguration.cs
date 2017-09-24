using Framework.Infrastructure.Config;

namespace LogR.Common.Interfaces.Service.Config
{
    public interface IAppConfiguration : IBaseConfiguration
    {
        int ServerPort { get; }

        string IndexBaseFolder { get; }

        string AppLogIndexFolder { get; }

        string PerformanceLogIndexFolder { get; }
    }
}
