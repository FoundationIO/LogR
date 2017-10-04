using Framework.Infrastructure.Config;
using Framework.Infrastructure.Models.Config;
using LogR.Common.Enums;
using LogR.Common.Models.Config;

namespace LogR.Common.Interfaces.Service.Config
{
    public interface IAppConfiguration : IBaseConfiguration
    {
        int BatchSizeToIndex { get; }

        IndexStoreType IndexStoreType { get; }

        LuceneIndexStoreSettings LuceneIndexStoreSettings { get; }

        DbSettings SqlIndexStoreSettings { get; }

        ElasticSearchIndexStoreSettings ElasticSearchIndexStoreSettings { get; }

        //EmbeddedElasticSearchIndexStoreSettings EmbeddedElasticSearchIndexStoreSettings { get; }

        RaptorDBIndexStoreSettings RaptorDBIndexStoreSettings { get;  }

        MongoDBIndexStoreSettings MongoDBIndexStoreSettings { get; }

        int ServerPort { get; }

        bool IsSqlBasedIndexStore();
    }
}
