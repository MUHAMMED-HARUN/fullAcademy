using FluentValidation;
using System.Application.Services.Enrollment.StudentEnrollment.Command;

namespace System.Application.Services.Enrollment.StudentEnrollment.Validations
{
    public sealed class NewStudentEnrollmentCommandValidation
        : AbstractValidator<NewStudentEnrollmentCommand>
    {
        public NewStudentEnrollmentCommandValidation()
        {
            RuleFor(command => command.StudentAcademicEnrollmentID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentAcademicEnrollmentID must be a valid, non-empty GUID");
            RuleFor(command => command.GroupID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("GroupID must be a valid, non-empty GUID");
            RuleFor(command => command.Date)
                .Must(date => date != default)
                .WithMessage("A valid Date is required");
        }
    }
}
