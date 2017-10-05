using System;

namespace Framework.Infrastructure.Interfaces.DbAccess
{
    public interface ITransaction : IDisposable
    {
        int CurrentTransactionCount { get; }

        int Complete();
    }
}
