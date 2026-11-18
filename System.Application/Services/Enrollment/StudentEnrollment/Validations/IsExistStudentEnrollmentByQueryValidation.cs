using FluentValidation;
using System.Application.Services.Enrollment.StudentEnrollment.Query;

namespace System.Application.Services.Enrollment.StudentEnrollment.Validations
{
    public sealed class IsExistStudentEnrollmentByQueryValidation
        : AbstractValidator<IsExistStudentEnrollmentByQuery>
    {
        public IsExistStudentEnrollmentByQueryValidation()
        {
            RuleFor(query => query.Param)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("Param must be a valid, non-empty GUID");
            RuleFor(query => query.ParamType).IsInEnum();
        }
    }
}