using AutoMapper;
using System.Application.Common;
using System.Application.Services.Assignment.Assignment.Command;
using System.Application.Services.Assignment.AssignmentScope.Command;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentScope.Mapper
{
    public class AssignmentScopeProfile : Profile
    {
        public AssignmentScopeProfile()
        {
            CreateMap<NewAssignmentScopeCommand, clsAssignmentScope>()
                .ConstructUsing(src => new clsAssignmentScope(
                    src.Name,
                    src.Notes,
                    clsUtil.ToGuid(src.StageID),
                    clsUtil.ToGuid(src.PrevAssignmentScopeID)
                ))
                .ForAllMembers(opt => opt.Ignore());


            var UpdateMap = CreateMap<UpdateAssignmentScopeCommand, clsAssignmentScope>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
                {
                    dest.SetName(src.Name);
                    dest.SetNotes(src.Notes);
                    dest.SetStageID(clsUtil.ToGuid(src.StageID));
                    dest.SetPrevAssignmentScopeID(clsUtil.ToGuid( src.PrevAssignmentScopeID));

                });
        }
    }
}
