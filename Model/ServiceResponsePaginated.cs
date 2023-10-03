namespace WebAPI6_Demo.Model
{
    public class ServiceResponsePaginated<T> : ServiceResponse<T>
    { 
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages { get; set; }
        public int? PreviousPage { get; set; }
        public int? NextPage { get; set; }
    }
}
