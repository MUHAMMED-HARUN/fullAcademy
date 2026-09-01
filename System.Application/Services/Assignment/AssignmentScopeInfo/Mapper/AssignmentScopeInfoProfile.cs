using AutoMapper;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentScope.Command;
using System.Application.Services.Assignment.AssignmentScopeInfo.Command;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Mapper
{
    public class AssignmentScopeInfoProfile : Profile
    {
        public AssignmentScopeInfoProfile()
        {
            CreateMap<NewAssignmentScopeInfoCommand, clsAssignmentScopeinfo>()
                .ConstructUsing(src => new clsAssignmentScopeinfo(
                    src.MaxScore,
                    src.MinPassScore,
                    src.Weight,
                    clsUtil.ToGuid(src.StageInfoID),
                    clsUtil.ToGuid(src.AssignmentScopeID)
                ))
                .ForAllMembers(opt => opt.Ignore());


            var UpdateMap = CreateMap<UpdateAssignmentScopeInfoCommand, clsAssignmentScopeinfo>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
                {
                    dest.SetMaxScore(src.MaxScore);
                    dest.SetMinPassScore(src.MinPassScore);
                    dest.SetWeight(src.Weight);
                    dest.SetStageInfoID(clsUtil.ToGuid(src.StageInfoID));
                    dest.SetAssignmentScopeID(clsUtil.ToGuid(src.AssignmentScopeID));

                });
        }
    }
}
