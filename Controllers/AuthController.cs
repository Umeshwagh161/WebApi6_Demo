using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI6_Demo.Dtos.AuthDto;
using WebAPI6_Demo.Model;
using WebAPI6_Demo.Service.AuthService;

namespace WebAPI6_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto registerRequest)
        {
            var response = await _authService.Register(registerRequest);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(LoginDto loginRequest)
        {
            var response = await _authService.Login(loginRequest);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        //Refresh token used after main token is expired.
        //When main token expired and if you don't have refresh token then it will redirect you to login.

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<AuthenticationResponse>> RefreshToken()
        {
            var response = await _authService.RefreshToken();
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult<AuthenticationResponse>> Logout()
        {
            var response = await _authService.Logout();
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        //This mathod accessible for Admin and Teacher only
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost("Resetpassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ResetPassword(ResetPasswordDto resetPasswordRequest)
        {
            var response = await _authService.ResetPassword(resetPasswordRequest);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }
    }
}
