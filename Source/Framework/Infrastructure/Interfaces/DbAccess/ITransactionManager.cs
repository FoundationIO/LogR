namespace Framework.Infrastructure.Interfaces.DbAccess
{
    public interface ITransactionManager
    {
        ITransaction BeginTransaction();
    }
}
