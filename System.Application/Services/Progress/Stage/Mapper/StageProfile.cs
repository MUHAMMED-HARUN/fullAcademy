using AutoMapper;
using System.Application.Common;
using System.Application.Services.Progress.Stage.Command;
using System.Domain.Entities.Progress;

namespace System.Application.Services.Progress.Stage.Mapper
{
    public class StageProfile : Profile
    {
        public StageProfile()
        {
            CreateMap<NewStageCommand, clsStage>()
                  .ConstructUsing(src => new clsStage(src.StageName, src.StageNote, clsUtil.ToGuid(src.ClassID),
                      clsUtil.ToGuid(src.PrevStageID))
                  ).ForAllMembers(opt => opt.Ignore()); 

            var UpdateMap = CreateMap<UpdateStageCommand, clsStage>();
            UpdateMap.ForAllMembers(opt => opt.Ignore());
            UpdateMap
                .AfterMap((src, dest) =>
            {
                dest.SetID(clsUtil.ToGuid(src.StageID));
                dest.SetName(src.StageName);
                dest.SetNotes(src.StageNote);
                dest.SetClassID(clsUtil.ToGuid(src.ClassID));
                dest.SetPrevStageID(clsUtil.ToGuid(src.PrevStageID));
            }
                );
        }
    }
}
