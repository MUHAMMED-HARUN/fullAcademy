using FluentValidation;
using System.Application.Services.Assignment.Assignment.Command;

namespace System.Application.Services.Assignment.AssignmentNomination.Validation
{
    public class UpdateAssignmentNominationCommandValidation : AbstractValidator<UpdateAssignmentNominationCommand>
    {
        public UpdateAssignmentNominationCommandValidation()
        {
            RuleFor(x => x.AssignmentNominationID)
    .NotEmpty().WithMessage("AssignmentNomination ID is required");

            RuleFor(x => x.StudentEnrollmentID)
                .NotEmpty().WithMessage("Student Enrollment ID is required");

            RuleFor(x => x.AssignmentScopeID)
                .NotEmpty().WithMessage("Assignment Scope ID is required");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required");

        }
    }
}
