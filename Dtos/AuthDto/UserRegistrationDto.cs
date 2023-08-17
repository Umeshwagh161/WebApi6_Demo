using System.ComponentModel.DataAnnotations;

namespace WebAPI6_Demo.Dtos.AuthDto
{
    public class UserRegisterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
