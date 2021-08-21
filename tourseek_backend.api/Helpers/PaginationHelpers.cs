using System.Linq;
using tourseek_backend.domain.Models;
using tourseek_backend.domain.Models.Filters;
using tourseek_backend.domain.Models.Responses;

namespace tourseek_backend.api.Helpers
{
    public static class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(this PaginationFilter pagination, PagedList<T> pagedData)
        {
            if (!pagination.Paging) pagination.PageNumber = 0;

            pagination.PageSize = pagedData.Data.Count();

            return new PagedResponse<T>
            {
                Data = pagedData.Data,
                PageNumber = pagination.PageNumber >= 1 ? pagination.PageNumber : (int?)null,
                PageSize = pagination.PageSize >= 1 ? pagination.PageSize : (int?)null,
                PageLength = pagedData.PageLength,
                TotalLength = pagedData.TotalLength,
            };
        }
    }
}