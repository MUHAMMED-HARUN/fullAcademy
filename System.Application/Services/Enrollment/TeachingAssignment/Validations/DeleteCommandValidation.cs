using FluentValidation;
using System.Application.Services.Enrollment.TeachingAssignment.Command;

namespace System.Application.Services.Enrollment.TeachingAssignment.Validations
{
    public sealed class DeleteTeachingAssignmentCommandValidation
        : AbstractValidator<DeleteTeachingAssignmentCommand>
    {
        public DeleteTeachingAssignmentCommandValidation()
        {
            RuleFor(command => command.TeachingAssignmentID)
                .Must(value => string.IsNullOrWhiteSpace(value) ||
                      (Guid.TryParse(value, out var id) && id != Guid.Empty))
                .WithMessage("TeachingAssignmentID must be a valid, non-empty GUID");
            RuleFor(command => command.EnrollmentID)
                .Must(value => string.IsNullOrWhiteSpace(value) ||
                      (Guid.TryParse(value, out var id) && id != Guid.Empty))
                .WithMessage("EnrollmentID must be a valid, non-empty GUID");
            RuleFor(command => command)
                .Must(command =>
                    Guid.TryParse(command.ResolveID(), out var id) && id != Guid.Empty)
                .WithMessage("Teaching assignment ID is required");
        }
    }
}