using FluentValidation;
using System.Application.Services.Assignment.AssignmentNomination.Command;

namespace System.Application.Services.Assignment.AssignmentNomination.Validations
{
    public class NewAssignmentNominationCommandValidation : AbstractValidator<NewAssignmentNominationCommand>
    {
        public NewAssignmentNominationCommandValidation()
        {
            RuleFor(x => x.StudentAcademicEnrollmentID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentAcademicEnrollmentID must be a valid GUID");

            RuleFor(x => x.AssignmentScopeID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AssignmentScopeID must be a valid GUID");

            RuleFor(x => x.Date)
                .NotEmpty().Must(value => DateOnly.TryParse(value, out _))
                .WithMessage("Date must be a valid date");

        }
    }
}
