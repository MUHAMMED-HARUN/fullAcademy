using AutoMapper;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentScopeInfo.Command;
using System.Application.Services.Assignment.Result.Command;
using System.Domain.Entities;

namespace System.Application.Services.Assignment.Result.Mapper
{
    public class ResultProfile : Profile
    {
        public ResultProfile()
        {
            CreateMap<NewResultCommand, clsResult>()
                .ConstructUsing(src => new clsResult(
                    clsUtil.ToGuid(src.AssignmentID),
                    clsUtil.ToGuid(src.AssignmentScopeInfoID),
                    src.Score,
                    src.Notes
                ))
                .ForAllMembers(opt => opt.Ignore());


            var UpdateMap = CreateMap<UpdateResultCommand, clsResult>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
                {
     
                    dest.SetNotes(src.Notes);
                    dest.SetScore(src.Score);
                    dest.SetAssignmentID(clsUtil.ToGuid(src.AssignmentID));
                    dest.SetAssignmentScopeInfoID(clsUtil.ToGuid(src.AssignmentScopeInfoID));

                });
        }
    }
}
