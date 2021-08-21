using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace tourseek_backend.util.Extensions
{
    public static class QueryableExtension
    {
        public static IEnumerable<string> GetColumnNames<T>(this IQueryable<T> queryable)
        {
            return queryable.ElementType.GetProperties().Select(s => s.Name);
        }

        public static IEnumerable<string> GetColumnNames<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.AsQueryable().ElementType.GetProperties().Select(s => s.Name);
        }

        public static IEnumerable<dynamic> DynamicSelectAsync<T>(this IQueryable<T> queryable, ICollection<string> columns)
        {
            return queryable.Select("new(" + string.Join(',', columns) + ")").AsEnumerable();
        }

        public static IEnumerable<dynamic> DynamicSelect<T>(this IQueryable<T> queryable, ICollection<string> columns)
        {
            return queryable.Select("new(" + string.Join(',', columns) + ")").AsEnumerable();
        }
    }
}