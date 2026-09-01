using AutoMapper;
using System.Application.Common;
using System.Application.Services.Assignment.Assignment.Command;
using System.Application.Services.Assignment.AssignmentNomination.Command;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentNomination.Mapper
{
    public class AssignmentNominationProfile : Profile
    {
        public AssignmentNominationProfile()
        {
            CreateMap<NewAssignmentNominationCommand, clsAssignmentNomination>()
                .ConstructUsing(src => new clsAssignmentNomination(
                    clsUtil.ToGuid(src.StudentEnrollmentID),
                    clsUtil.ToGuid(src.AssignmentScopeID),
                    DateOnly.Parse(src.Date),
                    clsUtil.ToGuid(src.NominatedByID),
                    src.NominatedByType,
                    src.NominationState
                ))
                .ForAllMembers(opt => opt.Ignore());

            var UpdateMap = CreateMap<UpdateAssignmentNominationCommand, clsAssignmentNomination>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
                {
                    dest.SetStudentEnrollmentID(clsUtil.ToGuid(src.StudentEnrollmentID));
                    dest.SetAssignmentScopeID(clsUtil.ToGuid(src.AssignmentScopeID));
                    dest.SetDate(src.Date);
                    dest.SetNominatedByID(clsUtil.ToGuid( src.NominatedByID));
                    dest.SetNominatedByType(src.NominatedByType);
                    dest.SetNominationState(src.NominationState);

                });
        }
    }
}
