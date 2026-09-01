using AutoMapper;
using System.Application.Common;
using System.Application.Services.Progress.StageInfo.Command;
using System.Domain.Entities;

namespace System.Application.Services.Progress.StageInfo.Mapper
{
    public class StageInfoProfile : Profile
    {
        public StageInfoProfile()
        {
            CreateMap<NewStageInfoCommand, clsStageInfo>()
                  .ConstructUsing(src => new clsStageInfo(src.From,src.To,clsUtil.ToGuid(src.StageID),src.UniteType,src.ProgressType,clsUtil.ToGuid( src.MatterID))
                  );

            var UpdateMap = CreateMap<UpdateStageInfoCommand, clsStageInfo>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetID(clsUtil.ToGuid(src.StageInfoID));
                dest.SetFrom(src.From);
                dest.SetTo(src.To);
                dest.SetUniteType(src.UniteType);
                dest.SetProgressType(src.ProgressType);
                dest.SetStageID(clsUtil.ToGuid(src.StageID));
                dest.SetMatterID (clsUtil.ToGuid(src.MatterID));
            }
                );
        }
    }
}
