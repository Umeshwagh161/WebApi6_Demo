using WebAPI6_Demo.Model.BaseModel;

namespace WebAPI6_Demo.Model
{

    public class User:EntityWithAudit
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? ImgUrl { get; set; } = string.Empty;
        public string? ImgName { get; set; } = string.Empty;
        public string PasswordHashed { get; set; } = string.Empty;
        public int? RoleId { get; set; }
        public virtual Role? Role { get; set; }      
        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
