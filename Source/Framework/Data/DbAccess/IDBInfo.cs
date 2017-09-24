using FluentMigrator.Runner.Processors;
using LinqToDB.DataProvider;

namespace Framework.Data.DbAccess
{
    public interface IDBInfo
    {
        string GetConnectionString();

        IDataProvider GetDBProvider();

        MigrationProcessorFactory GetMigrationProcessorFactory();
    }
}