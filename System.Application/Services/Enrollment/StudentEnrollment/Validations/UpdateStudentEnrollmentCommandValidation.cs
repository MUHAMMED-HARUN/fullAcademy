using FluentValidation;
using System.Application.Services.Enrollment.StudentEnrollment.Command;

namespace System.Application.Services.Enrollment.StudentEnrollment.Validations
{
    public sealed class UpdateStudentEnrollmentCommandValidation
        : AbstractValidator<UpdateStudentEnrollmentCommand>
    {
        public UpdateStudentEnrollmentCommandValidation()
        {
            RuleFor(command => command.EnrollmentID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("EnrollmentID must be a valid, non-empty GUID");
            RuleFor(command => command.Date)
                .Must(date => date != default)
                .WithMessage("A valid Date is required");
        }
    }
}
