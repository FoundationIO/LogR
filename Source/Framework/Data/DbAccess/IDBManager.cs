using LinqToDB.Data;

namespace Framework.Data.DbAccess
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