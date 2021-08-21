using System.Collections.Generic;

namespace tourseek_backend.domain.Models.Responses
{
    public class PagedResponse<T>
    {
        public PagedResponse(){}

        public PagedResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public IEnumerable<T> Data { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int PageLength { get; set; }
        public int TotalLength { get; set; }
    }
}