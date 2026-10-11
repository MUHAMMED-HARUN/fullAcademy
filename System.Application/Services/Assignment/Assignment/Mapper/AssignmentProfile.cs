using AutoMapper;
using System.Application.Common;
using System.Application.Services.Assignment.Assignment.Command;
using System.Application.Services.Enrollment.StudentEnrollment.Command;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.Assignment.Mapper
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            CreateMap<NewAssignmentCommand, clsAssignment>()
                .ConstructUsing(src => new clsAssignment(
                    clsUtil.ToGuid(src.AssignmentNominationID),
                    clsUtil.ToGuid(src.CommitteeID),
                    src.CommitteeType,
                    DateOnly.Parse(src.ScheduledDate),
                    src.AssignmentState,
                    src.IsPassed,
                    null
                ))
                .ForAllMembers(opt => opt.Ignore());


            var UpdateMap = CreateMap<UpdateAssignmentCommand, clsAssignment>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
                {
                    dest.SetAssignmentNominationID(clsUtil.ToGuid(src.AssignmentNominationID));
                    dest.SetCommitteeID(clsUtil.ToGuid(src.CommitteeID));
                    dest.SetCommitteeType(src.CommitteeType);
                    dest.SetScheduledDate(src.ScheduledDate);
                    dest.SetAssignmentState(src.AssignmentState);
                    dest.SetIsPassed(src.IsPassed);

                });
        }
    }
}
