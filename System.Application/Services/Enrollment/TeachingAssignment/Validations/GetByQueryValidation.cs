using FluentValidation;
using System.Application.Services.Enrollment.TeachingAssignment.Query;

namespace System.Application.Services.Enrollment.TeachingAssignment.Validations
{
    public sealed class GetTeachingAssignmentByQueryValidation
        : AbstractValidator<GetTeachingAssignmentByQuery>
    {
        public GetTeachingAssignmentByQueryValidation()
        {
            RuleFor(query => query.Param)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("Param must be a valid, non-empty GUID");
            RuleFor(query => query.ParamType).IsInEnum();
        }
    }
}