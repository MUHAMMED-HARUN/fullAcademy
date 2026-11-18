using FluentValidation;
using System.Application.Services.Enrollment.StudentEnrollment.Command;

namespace System.Application.Services.Enrollment.StudentEnrollment.Validations
{
    public sealed class DeleteStudentEnrollmentCommandValidation
        : AbstractValidator<DeleteStudentEnrollmentCommand>
    {
        public DeleteStudentEnrollmentCommandValidation()
        {
            RuleFor(command => command.EnrollmentID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("EnrollmentID must be a valid, non-empty GUID");
        }
    }
}