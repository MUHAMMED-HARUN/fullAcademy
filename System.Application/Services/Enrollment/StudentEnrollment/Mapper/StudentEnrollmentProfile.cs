using AutoMapper;
using System.Application.Common;
using System.Application.Services.Enrollment.StudentEnrollment.Command;
using System.Domain.Entities;

namespace System.Application.Services.Enrollment.StudentEnrollment.Mapper
{
    public class StudentEnrollmentProfile : Profile
    {
        public StudentEnrollmentProfile()
        {
            var UpdateMap = CreateMap<UpdateStudentEnrollmentCommand, clsStudentEnrollment>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
                {
                    dest.SetDate(src.Date);
                });
        }
    }
}
