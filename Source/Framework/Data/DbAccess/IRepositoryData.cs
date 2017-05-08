using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LinqToDB;

namespace Framework.Data.DbAccess
{
    public interface IRepositoryData<T> where T : class, new()
    {
        ITable<T> Table { get; }

        void BeginTransaction();
        void CommitTransaction();
        long Count();
        long Count(Expression<Func<T, bool>> predicate);
        int Delete(Expression<Func<T, bool>> where);
        void Dispose();
        T First(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        void Insert(IEnumerable<T> objs);
        void Insert(params T[] objs);
        void Insert(T obj);
        void InsertAll(List<T> list);
        void RollbackTransaction();
        List<T> Select(Expression<Func<T, bool>> predicate);
        List<T> SelectAll();
        List<T> SelectByPage(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate = null);
        List<T> SelectByPage(int pageNumber, int pageSize, out long totalItems, Expression<Func<T, bool>> predicate = null);
        void Update(IEnumerable<T> objs);
        void Update(params T[] objs);
        void Update(T obj);
    }
}