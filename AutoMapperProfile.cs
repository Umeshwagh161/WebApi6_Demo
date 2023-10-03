using AutoMapper;
using WebAPI6_Demo.Dtos;
using WebAPI6_Demo.Model;

namespace WebAPI6_Demo
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            //Convert  stundet to AddStudentDto;
            CreateMap<Student,StudentDto>();
        }
    }
}
