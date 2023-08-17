using WebAPI6_Demo.Model;

namespace WebAPI6_Demo.Dtos.AuthDto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ImgUrl { get; set; }
        public Role? Role { get; set; }
    }
}
