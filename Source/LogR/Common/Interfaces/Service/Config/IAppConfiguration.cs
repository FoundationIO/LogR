using Framework.Infrastructure.Config;
using LogR.Common.Enums;
using LogR.Common.Models.Config;

namespace LogR.Common.Interfaces.Service.Config
{
    public interface IAppConfiguration : IBaseConfiguration
    {
        int BatchSizeToIndex { get; }

        IndexStoreType IndexStoreType { get; }

        LuceneIndexStoreSettings LuceneIndexStoreSettings { get; }

        Sqite3IndexStoreSettings Sqite3IndexStoreSettings { get; }

        SqlServerIndexStoreSettings SqlServerIndexStoreSettings { get; }

        MySqlIndexStoreSettings MySqlIndexStoreSettings { get; }

        PostgresqlIndexStoreSettings PostgresqlIndexStoreSettings { get; }

        ElasticSearchIndexStoreSettings ElasticSearchIndexStoreSettings { get; }

        EmbeddedElasticSearchIndexStoreSettings EmbeddedElasticSearchIndexStoreSettings { get; }

        RaptorDBIndexStoreSettings RaptorDBIndexStoreSettings { get;  }

        MongoDBIndexStoreSettings MongoDBIndexStoreSettings { get; }

        int ServerPort { get; }

        string IndexBaseFolder { get; }

        string AppLogIndexFolder { get; }

        string PerformanceLogIndexFolder { get; }

        bool IsSqlBasedIndexStore();
    }
}
