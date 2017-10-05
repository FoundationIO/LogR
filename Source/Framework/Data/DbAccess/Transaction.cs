using System;
using Framework.Infrastructure.Interfaces.DbAccess;

namespace Framework.Data.DbAccess
{
    public class Transaction : ITransaction
    {
        private IDBManager dbManager;
        private bool commitOrRollbackhandled = false;

        public Transaction(IDBManager dbManager)
        {
            this.dbManager = dbManager;
            CurrentTransactionCount = this.dbManager.BeginTransaction();
        }

        public int CurrentTransactionCount { get; protected set; } = 0;

        public int Complete()
        {
            if (commitOrRollbackhandled == true)
                throw new Exception("Commit of Rollback is already called");

            var result = dbManager.CommitTransaction();
            commitOrRollbackhandled = true;
            return result;
        }

        public int Rollback()
        {
            if (commitOrRollbackhandled == true)
                throw new Exception("Commit of Rollback is already called");

            var result = dbManager.RollbackTransaction();
            commitOrRollbackhandled = true;

            return result;
        }

        public void Dispose()
        {
            if (commitOrRollbackhandled == false)
                Rollback();
        }
    }
}
