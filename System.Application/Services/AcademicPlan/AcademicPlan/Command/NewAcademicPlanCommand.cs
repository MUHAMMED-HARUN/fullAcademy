using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Domain.Enums.AcademicPlan;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Command
{
    public class NewAcademicPlanCommand : ICommand, IRequest<TResult<string>>
    {
        public string Name { get; set; }
        public string ProviderInstituteID { get; set; }
        public enAcademicPlanScopeType ScopeType { get; set; }
        public string? ProgramID { get; set; }
        public string? InstituteClassID { get; set; }
        public string? StageID { get; set; }
        public string? StageInfoID { get; set; }
        public DateOnly EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }
        public string? Notes { get; set; }
    }
}
