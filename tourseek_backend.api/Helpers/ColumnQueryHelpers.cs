using System.Linq;
using tourseek_backend.api.Queries;

namespace tourseek_backend.api.Helpers
{
    public static class ColumnQueryHelpers
    {
        public static string[] GetColumns(this ColumnsQuery columnsQuery)
        {
            string[] columns = null;
            if (columnsQuery.Columns != null)
            {
                columns = columnsQuery.Columns.Split(',')
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .Select(c => c.Trim())
                    .ToArray();
                columns = columns.Length == 0 ? null : columns;
            }

            return columns;
        }
    }
}