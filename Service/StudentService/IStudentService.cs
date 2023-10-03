using WebAPI6_Demo.Dtos;
using WebAPI6_Demo.Filters;
using WebAPI6_Demo.Model;

namespace WebAPI6_Demo.Service.StudentService
{
    public interface IStudentService
    {
        Task<ServiceResponse<int>> AddStudent(StudentDto addRequest);
        Task<ServiceResponsePaginated<List<StudentDto>>> SearchStudents(PaginationFilters paginationFilter, string filters);
        Task<ServiceResponsePaginated<List<StudentDto>>> GetAllStudent(PaginationFilters paginationFilter);
        Task<ServiceResponsePaginated<List<StudentDto>>> SortColumn(PaginationFilters paginationFilter, string colName, string sortOrder);
    }
}
