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
                    clsUtil.ToGuid(src.AcademicPlanID),
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
                dest.SetAcademicPlanID(clsUtil.ToGuid(src.AcademicPlanID));
                dest.SetNotes(src.Notes);
            });
        }
    }
}
