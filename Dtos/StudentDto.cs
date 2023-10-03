using WebAPI6_Demo.Model;

namespace WebAPI6_Demo.Dtos
{
    public class StudentDto
    {   
        public string StudentName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Remark { get; set; } = string.Empty;

    }
}
