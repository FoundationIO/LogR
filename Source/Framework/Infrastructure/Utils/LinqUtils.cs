using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Infrastructure.Utils
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

        public static void ForEachWithIndex<T>(this IEnumerable<T> enumerable, Action<T, int> handler)
        {
            if (enumerable == null)
                return;
            var idx = 0;
            foreach (var item in enumerable)
                handler(item, idx++);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
                return;
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
    }
}
