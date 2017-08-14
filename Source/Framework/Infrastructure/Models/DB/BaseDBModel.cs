using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Models.DB
{
    public abstract class BaseDBModel<T>
    {
        abstract public object GetPrimaryKeyValue();
        abstract public string GetPrimaryKeyName();
        abstract public void SetPrimaryKeyValue(object pkValue);
        abstract public string GetTableName();
        abstract public IQueryable<T> OrderByPrimaryKey(IQueryable<T> source, bool isAsc = true);
        abstract public IQueryable<T> OrderByKey(IQueryable<T> source, string key, bool isAsc = true);
        abstract public Expression<Func<T, bool>> PrimaryKeySelectExpression(object value);
        abstract public Expression<Func<T, bool>> KeySelectExpression(string key, object value);

    }
}