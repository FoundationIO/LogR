using System;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Infrastructure.Models.DB
{
    public abstract class BaseDBModel<T>
    {
        public abstract object GetPrimaryKeyValue();

        public abstract string GetPrimaryKeyName();

        public abstract void SetPrimaryKeyValue(object pkValue);

        public abstract string GetTableName();

        public abstract IQueryable<T> OrderByPrimaryKey(IQueryable<T> source, bool isAsc = true);

        public abstract IQueryable<T> OrderByKey(IQueryable<T> source, string key, bool isAsc = true);

        public abstract Expression<Func<T, bool>> PrimaryKeySelectExpression(object value);

        public abstract Expression<Func<T, bool>> KeySelectExpression(string key, object value);
    }
}