using AutoMapper;
using System.Application.Common;
using System.Application.Services.Finance.FeeStructure.Command;
using System.Domain.Entities.Finance;

namespace System.Application.Services.Finance.FeeStructure.Mapper
{
    public class FeeStructureProfile : Profile
    {
        public FeeStructureProfile()
        {
            CreateMap<NewFeeStructureCommand, clsFeeStructure>()
                .ConstructUsing(src => new clsFeeStructure(
                    src.Name,
                    src.FeeType,
                    src.Amount,
                    !string.IsNullOrEmpty(src.ProgramID) ? clsUtil.ToGuid(src.ProgramID) : null,
                    !string.IsNullOrEmpty(src.ClassID) ? clsUtil.ToGuid(src.ClassID) : null,
                    !string.IsNullOrEmpty(src.StageID) ? clsUtil.ToGuid(src.StageID) : null,
                    !string.IsNullOrEmpty(src.StageInfoID) ? clsUtil.ToGuid(src.StageInfoID) : null,
                    src.Notes
                ))
                .ForAllMembers(opt => opt.Ignore());

            var updateMap = CreateMap<UpdateFeeStructureCommand, clsFeeStructure>();
            updateMap.ForAllMembers(opt => opt.Ignore());
            updateMap.AfterMap((src, dest) =>
            {
                dest.SetName(src.Name);
                dest.SetFeeType(src.FeeType);
                dest.SetAmount(src.Amount);
                dest.SetProgramID(!string.IsNullOrEmpty(src.ProgramID) ? clsUtil.ToGuid(src.ProgramID) : null);
                dest.SetClassID(!string.IsNullOrEmpty(src.ClassID) ? clsUtil.ToGuid(src.ClassID) : null);
                dest.SetStageID(!string.IsNullOrEmpty(src.StageID) ? clsUtil.ToGuid(src.StageID) : null);
                dest.SetStageInfoID(!string.IsNullOrEmpty(src.StageInfoID) ? clsUtil.ToGuid(src.StageInfoID) : null);
                dest.SetNotes(src.Notes);
            });
        }
    }
}
