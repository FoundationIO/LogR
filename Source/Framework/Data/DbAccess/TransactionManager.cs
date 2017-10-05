using Framework.Infrastructure.Interfaces.DbAccess;

namespace Framework.Data.DbAccess
{
    public class TransactionManager : ITransactionManager
    {
        private IDBManager dbManager;

        public TransactionManager(IDBManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public ITransaction BeginTransaction()
        {
            return new Transaction(dbManager);
        }
    }
}
