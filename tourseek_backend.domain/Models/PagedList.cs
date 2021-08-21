using System.Collections.Generic;

namespace tourseek_backend.domain.Models
{
    public class PagedList<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalLength { get; set; }
        public int PageLength { get; set; }
    }
}