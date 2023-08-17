using WebAPI6_Demo.Dtos.AuthDto;
using WebAPI6_Demo.Model;

namespace WebAPI6_Demo.Service.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegisterDto registerRequest);
        Task<AuthenticationResponse> Login(LoginDto loginRequest);
        Task<AuthenticationResponse> RefreshToken();
        Task<AuthenticationResponse> Logout();
        Task<ServiceResponse<string>> ResetPassword(ResetPasswordDto resetPasswordRequest);
        Task<ServiceResponse<bool>> UserExists(string email);
    }
}
