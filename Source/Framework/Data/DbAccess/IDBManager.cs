using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LinqToDB;
using LinqToDB.Data;

namespace Framework.Data.DbAccess
{
    public interface IDBManager
    {
        string ConnectionString { get; set; }

        DataConnection Connection { get; }

        int BeginTransaction();

        int CommitTransaction();

        void Dispose();

        int RollbackTransaction();

        long Count<T>()
            where T : class;

        long Count<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        int Delete<T>(Expression<Func<T, bool>> where)
            where T : class;

        T First<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        T FirstOrDefault<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        BulkCopyRowsCopied BulkCopy<T>(IEnumerable<T> lst)
            where T : class;

        void Insert<T>(IEnumerable<T> objs)
            where T : class;

        void Insert<T>(T obj)
            where T : class;

        void InsertAll<T>(List<T> list)
            where T : class;

        List<T> Select<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        List<T> SelectAll<T>()
            where T : class;

        List<T> SelectByPage<T>(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate = null)
            where T : class;

        List<T> SelectByPage<T>(int pageNumber, int pageSize, out long totalItems, Expression<Func<T, bool>> predicate = null)
            where T : class;

        void Update<T>(IEnumerable<T> objs)
            where T : class;

        void Update<T>(T obj)
            where T : class;
    }
}