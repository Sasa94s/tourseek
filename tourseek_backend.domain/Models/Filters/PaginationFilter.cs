namespace tourseek_backend.domain.Models.Filters
{
    public class PaginationFilter
    {
        public bool Paging { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}