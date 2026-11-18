using MediatR;
using System.Application.Abstractions;
using System.Application.Common;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Command
{
    public class UpdateAcademicPlanCommand : ICommand, IRequest<TResult<string>>
    {
        public string AcademicPlanID { get; set; }
        public string Name { get; set; }
        public DateOnly EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }
        public string? Notes { get; set; }
    }
}
