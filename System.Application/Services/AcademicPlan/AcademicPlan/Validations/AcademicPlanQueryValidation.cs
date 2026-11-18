using FluentValidation;
using System.Application.Services.AcademicPlan.AcademicPlan.Query;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Validations
{
    public sealed class GetAllAcademicPlanFilterQueryValidation : AbstractValidator<GetAllAcademicPlanFilterQuery>
    {
        public GetAllAcademicPlanFilterQueryValidation()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
            RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
        }
    }

    public sealed class GetAcademicPlanByQueryValidation : AbstractValidator<GetAcademicPlanByQuery>
    {
        public GetAcademicPlanByQueryValidation()
        {
            RuleFor(query => query.AcademicPlanID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AcademicPlanID is required");
        }
    }
}
