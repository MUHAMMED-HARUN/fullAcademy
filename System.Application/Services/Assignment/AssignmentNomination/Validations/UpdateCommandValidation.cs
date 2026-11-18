using FluentValidation;
using System.Application.Services.Assignment.AssignmentNomination.Command;

namespace System.Application.Services.Assignment.AssignmentNomination.Validations
{
    public class UpdateAssignmentNominationCommandValidation : AbstractValidator<UpdateAssignmentNominationCommand>
    {
        public UpdateAssignmentNominationCommandValidation()
        {
            RuleFor(x => x.AssignmentNominationID)
    .NotEmpty().WithMessage("AssignmentNomination ID is required");

            RuleFor(x => x.StudentAcademicEnrollmentID)
                .NotEmpty().Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentAcademicEnrollmentID must be a valid GUID");

            RuleFor(x => x.AssignmentScopeID)
                .NotEmpty().WithMessage("Assignment Scope ID is required");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required");

        }
    }
}
