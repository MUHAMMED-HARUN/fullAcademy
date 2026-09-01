using FluentValidation;
using System.Application.Services.Enrollment.StudentEnrollment.Query;

namespace System.Application.Services.Enrollment.StudentEnrollment.Validation
{
    public class IsExistStudentEnrollmentByQueryValidation : AbstractValidator<IsExistStudentEnrollmentByQuery>
    {
        public IsExistStudentEnrollmentByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param must not be empty");
        }
    }
}
