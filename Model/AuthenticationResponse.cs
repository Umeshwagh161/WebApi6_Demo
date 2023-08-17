namespace WebAPI6_Demo.Model
{
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpires { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
