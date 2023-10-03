using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI6_Demo.Dtos;
using WebAPI6_Demo.Filters;
using WebAPI6_Demo.Model;

namespace WebAPI6_Demo.Service.StudentService
{
    public class StudentService:IStudentService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public StudentService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponsePaginated<List<StudentDto>>> GetAllStudent(PaginationFilters paginationFilter)
        {
            var response = new ServiceResponsePaginated<List<StudentDto>>();
            var validFilter = new PaginationFilters(paginationFilter.PageNumber, paginationFilter.PageSize);

            var student = await _context.Students
                .Where(o => o.IsDelete == false)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var totalRecords = await _context.Students.CountAsync();
            response = PagnationHelper.PagedResponse(response, validFilter, totalRecords);

            response.Success = true;
            response.Message = $"Found {student.Count} student.";
            response.Data = student.Select(c => _mapper.Map<StudentDto>(c)).ToList();
            return response;
        }   
        public async Task<ServiceResponsePaginated<List<StudentDto>>> SearchStudents(PaginationFilters paginationFilter, string filters)
        {
            var response = new ServiceResponsePaginated<List<StudentDto>>();
            var validFilter = new PaginationFilters(paginationFilter.PageNumber, paginationFilter.PageSize);

            IQueryable<Student> query = _context.Students
                .Where(o => o.IsDelete == false).Where(o=>o.Remark.ToLower().Contains(filters.ToLower()) || o.StudentName.ToLower().Contains(filters.ToLower()) || o.Age.ToString() == filters);

            var students = await query
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var totalRecords = await query.CountAsync();
            response = PagnationHelper.PagedResponse(response, validFilter, totalRecords);

            response.Success = true;
            response.Message = $"Found {totalRecords} student.";
            response.Data = students.Select(c => _mapper.Map<StudentDto>(c)).ToList();
            return response;
        }
        public async Task<ServiceResponsePaginated<List<StudentDto>>> SortColumn(PaginationFilters paginationFilter,string colName, string sortOrder)
        {
            var response = new ServiceResponsePaginated<List<StudentDto>>();
            var validFilter = new PaginationFilters(paginationFilter.PageNumber, paginationFilter.PageSize);

            IQueryable<Student> query=null;

            switch (colName)
            {
                case "StudentName":
                    if (sortOrder == "ASC") {query= _context.Students.OrderBy(x => x.StudentName); }
                    else { query =_context.Students.OrderByDescending(x => x.StudentName); }
                    break;

                case "Age":
                    if (sortOrder == "ASC") { query = _context.Students.OrderBy(x => x.Age); }
                    else { query = _context.Students.OrderByDescending(x => x.Age); }
                    break;

                case "Remark":
                    if (sortOrder == "ASC") { query = _context.Students.OrderBy(x => x.Remark); }
                    else { query = _context.Students.OrderByDescending(x => x.Remark); }
                    break;

                default:
                    if (sortOrder == "ASC") { query = _context.Students.OrderBy(x => x.Id); }
                    else { query = _context.Students.OrderByDescending(x => x.Id); }
                    break;

            }

            var students = await query
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var totalRecords = await query.CountAsync();

            response = PagnationHelper.PagedResponse(response, validFilter, totalRecords);

            response.Success = true;
            response.Message = $"Found {totalRecords} student.";
            response.Data = students.Select(c => _mapper.Map<StudentDto>(c)).ToList();
            return response;
        }
        public async Task<ServiceResponse<int>> AddStudent(StudentDto addRequest)
        {
            var response = new ServiceResponse<int>();
            Student student = new Student
            {
                StudentName = addRequest.StudentName,
                Age = addRequest.Age,
                Remark = addRequest.Remark,
                CreatedDate = DateTime.UtcNow
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            response.Message = $"Student Successfully Added {addRequest.StudentName}.";
            response.Success = true;
            return response;

        }
    }
}
