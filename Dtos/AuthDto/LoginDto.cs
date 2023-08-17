using System.ComponentModel.DataAnnotations;

namespace WebAPI6_Demo.Dtos.AuthDto
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
