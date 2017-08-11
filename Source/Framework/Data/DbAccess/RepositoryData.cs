using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using LinqToDB;
using LinqToDB.Linq;

namespace Framework.Data.DbAccess
{
    public class RepositoryData<T> : IDisposable, IRepositoryData<T> where T : class , new()
    {
        private readonly IDBManager dbMgr;
        public ITable<T> Table { get { return dbMgr.Connection.GetTable<T>(); } }

        public RepositoryData(IDBManager dbMgr)
        {
            this.dbMgr = dbMgr;
        }

        public void Dispose()
        {
            dbMgr.Dispose();
        }

        public int Delete(Expression<Func<T, bool>> where)
        {
            return Table.Delete(where);
        }

        public void Insert(T obj)
        {
            var identityValue = dbMgr.Connection.InsertWithIdentity<T>(obj);
        }

        public void Insert(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
            {
                Insert(obj);
            }
        }

        public void Update(T obj)
        {
            dbMgr.Connection.Update<T>(obj);
        }

        public void Update(IEnumerable<T> objs)
        {
            foreach (var obj in objs)
            {
                Update(obj);
            }
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return Table.First(predicate);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return Table.FirstOrDefault(predicate);
        }


        public void BeginTransaction()
        {
            dbMgr.BeginTransaction();
        }

        public void CommitTransaction()
        {
            dbMgr.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            dbMgr.RollbackTransaction();
        }

        public List<T> Select(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate).ToList();
        }

        public List<T> SelectAll()
        {
            return Table.ToList<T>();
        }

        public long Count(Expression<Func<T, bool>> predicate)
        {
            return Table.Count(predicate);
        }

        public long Count()
        {
            return Table.Count<T>();
        }

        public List<T> SelectByPage(int pageNumber, int pageSize, out long totalItems, Expression<Func<T, bool>> predicate = null)
        {
            if (pageSize == 0)
                pageSize = 100;
            if (pageNumber <= 0)
                pageNumber = 1;

            var querable = Table.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            if (predicate != null)
            {
                querable = querable.Where(predicate);
                totalItems = Table.Count(predicate);
            }
            else
            {
                totalItems = Table.Count();
            }
            return querable.ToList();
        }

        public List<T> SelectByPage(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate = null)
        {
            if (pageSize == 0)
                pageSize = 100;

            if (pageNumber <= 0)
                pageNumber = 1;

            var querable = Table.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            if (predicate != null)
            {
                querable = querable.Where(predicate);
            }
            return querable.ToList();
        }


        public void InsertAll(List<T> list)
        {
            foreach (var item in list)
            {
                Insert(item);
            }
        }
    }
}
