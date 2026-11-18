using FluentValidation;
using System.Application.Services.Enrollment.TeachingAssignment.Command;

namespace System.Application.Services.Enrollment.TeachingAssignment.Validations
{
    public sealed class UpdateTeachingAssignmentCommandValidation
        : AbstractValidator<UpdateTeachingAssignmentCommand>
    {
        public UpdateTeachingAssignmentCommandValidation()
        {
            RuleFor(command => command.EnrollmentID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("EnrollmentID must be a valid, non-empty GUID");
            RuleFor(command => command.TeacherID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("TeacherID must be a valid, non-empty GUID");
            RuleFor(command => command.GroupID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("GroupID must be a valid, non-empty GUID");
            RuleFor(command => command.SubjectID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("SubjectID must be a valid, non-empty GUID");
            RuleFor(command => command.Date).NotEmpty();
        }
    }
}