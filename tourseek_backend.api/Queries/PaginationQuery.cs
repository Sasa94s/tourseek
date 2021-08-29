namespace tourseek_backend.api.Queries
{
    public class PaginationQuery
    {
        public bool Paging { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationQuery()
        {
            Paging = true;
            PageNumber = 1;
            PageSize = 7;
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            Paging = true;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}