using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI6_Demo.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
