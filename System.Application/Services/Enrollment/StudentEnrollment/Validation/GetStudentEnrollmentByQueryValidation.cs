using FluentValidation;
using System.Application.Services.Enrollment.StudentEnrollment.Query;

namespace System.Application.Services.Enrollment.StudentEnrollment.Validation
{
    public class GetStudentEnrollmentByQueryValidation : AbstractValidator<GetStudentEnrollmentByQuery>
    {
        public GetStudentEnrollmentByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param must not be empty");
        }
    }
}
