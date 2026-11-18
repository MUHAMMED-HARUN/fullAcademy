using AutoMapper;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentNomination.Command;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentNomination.Mapper
{
    public class AssignmentNominationProfile : Profile
    {
        public AssignmentNominationProfile()
        {
            var updateMap = CreateMap<UpdateAssignmentNominationCommand, clsAssignmentNomination>();
            updateMap.ForAllMembers(opt => opt.Ignore());
            updateMap.AfterMap((src, dest) =>
            {
                dest.SetAssignmentScopeID(clsUtil.ToGuid(src.AssignmentScopeID));
                dest.SetDate(src.Date);
            });
        }
    }
}
