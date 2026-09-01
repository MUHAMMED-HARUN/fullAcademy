using FluentValidation;
using System.Application.Services.Enrollment.TeachingAssignment.Query;

namespace System.Application.Services.Enrollment.TeachingAssignment.Validation
{
    public class IsExistTeachingAssignmentByQueryValidation : AbstractValidator<IsExistTeachingAssignmentByQuery>
    {
        public IsExistTeachingAssignmentByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param must not be empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("Param TYPE must not be empty");
        }
    }
}
