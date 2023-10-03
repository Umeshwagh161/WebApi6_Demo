using WebAPI6_Demo.Model;

namespace WebAPI6_Demo.Filters
{
    public static class PagnationHelper
    {
        public static ServiceResponsePaginated<T> PagedResponse<T>(ServiceResponsePaginated<T> requestToPaginate, PaginationFilters validFilter, int totalRecords)
        {
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            int? nextPage = validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                ? validFilter.PageNumber + 1
                : null;
            int? prevPage = validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? validFilter.PageNumber - 1
                : null;

            requestToPaginate.PageNumber = validFilter.PageNumber;
            requestToPaginate.PageSize = validFilter.PageSize;
            requestToPaginate.TotalRecords = totalRecords;
            requestToPaginate.TotalPages = roundedTotalPages;
            requestToPaginate.PreviousPage = prevPage;
            requestToPaginate.NextPage = nextPage;

            return requestToPaginate;
        }
    }
}
