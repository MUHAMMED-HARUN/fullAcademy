using FluentValidation;
using System.Application.Services.Assignment.AssignmentNomination.Command;

namespace System.Application.Services.Assignment.AssignmentNomination.Validation
{
    public class NewAssignmentNominationCommandValidation : AbstractValidator<NewAssignmentNominationCommand>
    {
        public NewAssignmentNominationCommandValidation()
        {
            RuleFor(x => x.StudentEnrollmentID)
                .NotEmpty().WithMessage("Student Enrollment ID is required");

            RuleFor(x => x.AssignmentScopeID)
                .NotEmpty().WithMessage("Assignment Scope ID is required");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required");

        }
    }
}
