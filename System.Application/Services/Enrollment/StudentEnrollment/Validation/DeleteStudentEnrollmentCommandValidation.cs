using FluentValidation;
using System.Application.Services.Enrollment.StudentEnrollment.Command;

namespace System.Application.Services.Enrollment.StudentEnrollment.Validation
{
    public class DeleteStudentEnrollmentCommandValidation : AbstractValidator<DeleteStudentEnrollmentCommand>
    {
        public DeleteStudentEnrollmentCommandValidation()
        {
            RuleFor(x => x.EnrollmentID).NotEmpty().WithMessage("EnrollmentID must not be empty");
        }
    }
}
