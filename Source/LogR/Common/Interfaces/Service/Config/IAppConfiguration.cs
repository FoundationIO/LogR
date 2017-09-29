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

        SqlBasedIndexStoreSettings Sqite3IndexStoreSettings { get; }

        SqlBasedIndexStoreSettings SqlServerIndexStoreSettings { get; }

        SqlBasedIndexStoreSettings MySqlIndexStoreSettings { get; }

        SqlBasedIndexStoreSettings PostgresqlIndexStoreSettings { get; }

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
