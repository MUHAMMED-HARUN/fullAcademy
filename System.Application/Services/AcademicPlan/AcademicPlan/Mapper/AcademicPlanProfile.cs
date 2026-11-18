using AutoMapper;
using System.Application.Common;
using System.Application.Services.AcademicPlan.AcademicPlan.Command;
using System.Domain.Entities.AcademicPlan;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Mapper
{
    public class AcademicPlanProfile : Profile
    {
        public AcademicPlanProfile()
        {
            CreateMap<NewAcademicPlanCommand, clsAcademicPlan>()
                .ConstructUsing(src => new clsAcademicPlan(
                    src.Name,
                    clsUtil.ToGuid(src.ProviderInstituteID),
                    src.ScopeType,
                    !string.IsNullOrEmpty(src.ProgramID) ? clsUtil.ToGuid(src.ProgramID) : null,
                    !string.IsNullOrEmpty(src.InstituteClassID) ? clsUtil.ToGuid(src.InstituteClassID) : null,
                    !string.IsNullOrEmpty(src.StageID) ? clsUtil.ToGuid(src.StageID) : null,
                    !string.IsNullOrEmpty(src.StageInfoID) ? clsUtil.ToGuid(src.StageInfoID) : null,
                    src.EffectiveFrom,
                    src.EffectiveTo,
                    src.Notes,
                    1))
                .ForAllMembers(opt => opt.Ignore());
        }
    }
}
