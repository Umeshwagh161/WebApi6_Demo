using WebAPI6_Demo.Model.BaseModel;

namespace WebAPI6_Demo.Model
{
    public class RefreshToken:EntityWithAudit
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
