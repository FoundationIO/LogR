using LinqToDB.Data;

namespace Framework.Infrastructure
{
    public interface IDBManager
    {
        string ConnectionString { get; set; }
        DataConnection NewConnection { get; }

        void BeginTransaction();
        void CommitTransaction();
        void Dispose();
        void RollbackTransaction();
    }
}