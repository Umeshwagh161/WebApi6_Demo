namespace WebAPI6_Demo.Dtos.AuthDto
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
