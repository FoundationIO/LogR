using FluentMigrator.Runner.Processors;
using LinqToDB.DataProvider;

namespace Framework.Infrastructure
{
    public interface IDBInfo
    {
        string GetConnectionString();
        IDataProvider GetDBProvider();
        MigrationProcessorFactory GetMigrationProcessorFactory();
    }
}