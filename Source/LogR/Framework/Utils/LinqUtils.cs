using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Utils
{
    public static class LinqUtils
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> obj, int pageNumber, int pageSize)
        {
            if (pageSize == 0)
                pageSize = 100;

            if (pageNumber == 0)
                return obj.Take(pageSize);

            pageNumber = pageNumber - 1; //Skip records of previous pages only
            return obj.Skip(pageSize * pageNumber).Take(pageSize);
        }
    }
}
