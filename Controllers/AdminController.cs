using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI6_Demo.Dtos;
using WebAPI6_Demo.Filters;
using WebAPI6_Demo.Model;
using WebAPI6_Demo.Service.StudentService;

namespace WebAPI6_Demo.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IStudentService _studentService;

        public AdminController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("GetAllStudent")]
        public async Task<ActionResult<ServiceResponsePaginated<List<StudentDto>>>> GetAllStudent([FromQuery] PaginationFilters paginationFilter)
        {
            var response = await _studentService.GetAllStudent(paginationFilter);
            if (!response.Success) return BadRequest(response);
            return Ok(response);

        }

        [HttpGet("filter/search")]
        public async Task<ActionResult<ServiceResponsePaginated<List<StudentDto>>>> SearchStudents([FromQuery] PaginationFilters paginationFilter, string searchFilter)
        {
            var response = await _studentService.SearchStudents(paginationFilter, searchFilter);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("filter/sort")]
        public async Task<ActionResult<ServiceResponsePaginated<List<StudentDto>>>> SortColumn([FromQuery] PaginationFilters paginationFilter, string colName, string sortOrder)
        {
            var response = await _studentService.SortColumn(paginationFilter, colName, sortOrder);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("AddStudent")]
        public async Task<ActionResult<ServiceResponse<int>>> AddStudent(StudentDto registerRequest)
        {
            var response = await _studentService.AddStudent(registerRequest);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
      
    }
}
    
