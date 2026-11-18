using FluentValidation;
using System.Application.Services.Assignment.Assignment.Command;

namespace System.Application.Services.Assignment.Assignment.Validations
{
    public class UpdateAssignmentCommandValidation : AbstractValidator<UpdateAssignmentCommand>
    {
        public UpdateAssignmentCommandValidation()
        {
            RuleFor(x => x.AssignmentID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AssignmentID must be a valid GUID");
            RuleFor(x => x.ScheduledDate)
                .NotEmpty()
                .Must(value => DateOnly.TryParse(value, out _))
                .WithMessage("ScheduledDate must be a valid date");
        }
    }
}
