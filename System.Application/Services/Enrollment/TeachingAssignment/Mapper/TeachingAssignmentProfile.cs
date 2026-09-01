using AutoMapper;
using System.Application.Common;
using System.Application.Services.Enrollment.TeachingAssignment.Command;
using System.Domain.Entities;

namespace System.Application.Services.Enrollment.TeachingAssignment.Mapper
{
    public class TeachingAssignmentProfile : Profile
    {
        public TeachingAssignmentProfile()
        {
            CreateMap<NewTeachingAssignmentCommand, clsTeachingAssignment>()
                .ConstructUsing(src => new clsTeachingAssignment(
                    clsUtil.ToGuid(src.TeacherID),
                    clsUtil.ToGuid(src.GroupID),
                    clsUtil.ToGuid( src.SubjectID),
                    src.Date
                ));

            var UpdateMap = CreateMap<UpdateTeachingAssignmentCommand, clsTeachingAssignment>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
                {
                    dest.SetTeacherID(clsUtil.ToGuid(src.TeacherID));
                    dest.SetGroupID(clsUtil.ToGuid(src.GroupID));
                    dest.SetSubjectID(clsUtil.ToGuid(src.SubjectID));
                    dest.SetDate(src.Date);
                });
        }
    }
}
