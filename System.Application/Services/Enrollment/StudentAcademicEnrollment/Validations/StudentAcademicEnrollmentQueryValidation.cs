using FluentValidation;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.Query;

namespace System.Application.Services.Enrollment.StudentAcademicEnrollment.Validations
{
    public sealed class StudentAcademicEnrollmentQueryValidation : AbstractValidator<GetStudentAcademicEnrollmentByQuery>
    {
        public StudentAcademicEnrollmentQueryValidation()
        {
            RuleFor(x => x.StudentAcademicEnrollmentID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentAcademicEnrollmentID is required");
        }
    }
}
