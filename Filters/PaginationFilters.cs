namespace WebAPI6_Demo.Filters
{

    public class PaginationFilters
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PaginationFilters()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public PaginationFilters(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? pageSize : pageSize;
        }
    }

}
