namespace WebAPI6_Demo.Dtos.AuthDto
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
