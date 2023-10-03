using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using WebAPI6_Demo.Dtos.AuthDto;
using WebAPI6_Demo.Model;

namespace WebAPI6_Demo.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<int>> Register(UserRegisterDto registerRequest)
        {
            var response = new ServiceResponse<int>();

            var userExists = await UserExists(registerRequest.Email);
            if (userExists.Success)
            {
                response.Message ="Email alredy exist";
                response.Success = false;
            }
            else
            {
                User user = new User
                {
                    Email = registerRequest.Email.ToLower(),
                    Phone = registerRequest.Phone,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    PasswordHashed = HashPassword(registerRequest.Password),
                    CreatedDate = DateTime.UtcNow,
                    RoleId = 2
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                response.Message = $"Successfully registered {user.Email}.";
                response.Success = true;
            }

            return response;
        }
        public async Task<AuthenticationResponse> Login(LoginDto loginRequest)
        {
            AuthenticationResponse authenticationResponse=new AuthenticationResponse();
            var user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequest.Email.ToLower() && !u.IsDelete && u.IsActive);
            if (user == null)
            {
                authenticationResponse.Message = "User does not exist.";
                authenticationResponse.Success = false;
                return authenticationResponse;
            }
            else if (!VerifyPassword(loginRequest.Password, user.PasswordHashed))
            {
                authenticationResponse.Message = "Password does not match.";
                authenticationResponse.Success = false;
                return authenticationResponse;
            }
            else
            {
                var accessToken = GenerateToken(user);
                var refreshToken = GenerateRefreshToken(user);

                _context.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync();

                authenticationResponse.AccessToken = accessToken;
                authenticationResponse.RefreshToken = refreshToken.Token;
                authenticationResponse.RefreshTokenExpires = refreshToken.Expires;
                authenticationResponse.Message = $"Successfully logged in {user.Email}.";
                authenticationResponse.Success = true;
                SetRefreshToken(refreshToken);
                return authenticationResponse;
            }
        }
        public async Task<ServiceResponse<bool>> UserExists(string email)
        {
            var response = new ServiceResponse<bool>();
            var emailExists = await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
            if (!emailExists)
            {
                response.Success = false;
            }
            else
            {
                response.Success = true;
            }
            return response;
        }
        public async Task<AuthenticationResponse> Logout()
        {
            var response = new AuthenticationResponse();

            var refreshTokenInHttp = _httpContextAccessor?.HttpContext?.Request.Cookies["refreshToken"];
            var refreshTokenInDb = await _context.RefreshTokens.Include(u => u.User).ThenInclude(r => r.Role).FirstOrDefaultAsync(rt => rt.Token == refreshTokenInHttp);
            if (refreshTokenInDb == null)
            {
                _httpContextAccessor?.HttpContext?.Response.Cookies.Delete("refreshToken");

                response.Message = "Logged out, refresh Token is invalid,";
                response.Success = true;
            }
            else
            {
                _context.RefreshTokens.Remove(refreshTokenInDb);
                await _context.SaveChangesAsync();

                _httpContextAccessor?.HttpContext?.Response.Cookies.Delete("refreshToken");
                response.Message = "Logged out, removed refresh token.";
                response.Success = true;
            }

            return response;
        }
        public async Task<ServiceResponse<string>> ResetPassword(ResetPasswordDto resetPasswordRequest)
        {
            var response = new ServiceResponse<string>();
           

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordRequest.Email && u.IsActive && !u.IsDelete);

            if (user == null)
            {
                response.Message = "User does not exist.";
                response.Success = false;
            }
            else if (!VerifyPassword(resetPasswordRequest.OldPassword, user.PasswordHashed))
            {
                response.Message = "Old password does not match.";
                response.Success = false;
            }
            else
            {
                var newPassword = HashPassword(resetPasswordRequest.NewPassword);
                user.PasswordHashed = newPassword;
                await _context.SaveChangesAsync();

                response.Data = "Password";
                response.Message = "Password changed.";
                response.Success = true;
            }
            return response;
        }
        public async Task<AuthenticationResponse> RefreshToken()
        {
            var response = new AuthenticationResponse();
            var refreshTokenInHttp = _httpContextAccessor?.HttpContext?.Request.Cookies["refreshToken"];
            var refreshTokenInDb = await _context.RefreshTokens.Include(u => u.User).ThenInclude(r => r.Role).FirstOrDefaultAsync(rt => rt.Token == refreshTokenInHttp);
            if (refreshTokenInDb == null)
            {
                response.Message = "Refresh Token is invalid.";
                response.Success = false;
            }
            else if (refreshTokenInDb.Expires < DateTime.UtcNow)
            {
                refreshTokenInDb.IsActive = false;
                await _context.SaveChangesAsync();

                response.Message = "Refresh token has expired, please login again.";
                response.Success = false;
            }
            else
            {
                var accessToken = GenerateToken(refreshTokenInDb.User);
                var refreshToken = GenerateRefreshToken(refreshTokenInDb.User);

                refreshTokenInDb.Token = refreshToken.Token;
                refreshTokenInDb.Expires = refreshToken.Expires;
                refreshTokenInDb.CreatedDate = refreshToken.CreatedDate;
                await _context.SaveChangesAsync();

                response.AccessToken = accessToken;
                response.RefreshToken = refreshToken.Token;
                response.RefreshTokenExpires = refreshToken.Expires;
                response.Message = $"Successfully refreshed token for {refreshTokenInDb.User.Email}.";
                response.Success = true;

                SetRefreshToken(refreshToken);
            }

            return response;
        }

        //Convert you orginal password in BCrypt formate.
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }     
        private string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("email", user.Email),
            new Claim("phone", user.Phone),
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("roleId", user.Role.Id.ToString()),
        };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration.GetSection("AppSettings:JwtExpires").Value)),
                SigningCredentials = credentials,
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        private RefreshToken GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration.GetSection("AppSettings:RefreshExpires").Value)),
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                User = user,
            };

            return refreshToken;
        }
        private void SetRefreshToken(RefreshToken refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };
            _httpContextAccessor?.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
