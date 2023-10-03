using WebAPI6_Demo.Model.BaseModel;

namespace WebAPI6_Demo.Model
{
    public class Student : EntityWithAudit
    {
      public string StudentName { get; set; } = string.Empty;
      public int Age { get; set; }
      public string Remark { get; set; } = string.Empty;
    }
}
