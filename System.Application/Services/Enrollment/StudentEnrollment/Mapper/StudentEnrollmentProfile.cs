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
            CreateMap<NewStudentEnrollmentCommand, clsStudentEnrollment>()
                .ConstructUsing(src => new clsStudentEnrollment(
                    clsUtil.ToGuid(src.StudentID),
                    clsUtil.ToGuid(src.GroupID),
                    src.Date
                ));

            var UpdateMap = CreateMap<UpdateStudentEnrollmentCommand, clsStudentEnrollment>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
                {
                    dest.SetStudentID(clsUtil.ToGuid(src.StudentID));
                    dest.SetGroupID(clsUtil.ToGuid(src.GroupID));
                    dest.SetDate(src.Date);
                });
        }
    }
}
