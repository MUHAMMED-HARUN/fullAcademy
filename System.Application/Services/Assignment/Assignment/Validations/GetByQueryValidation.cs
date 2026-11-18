using FluentValidation;
using System.Application.Services.Assignment.Assignment.Query;

namespace System.Application.Services.Assignment.Assignment.Validations
{
    public sealed class GetAssignmentByQueryValidation : AbstractValidator<GetAssignmentByQuery>
    {
        public GetAssignmentByQueryValidation()
        {
            RuleFor(query => query.ID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AssignmentID must be a valid GUID");
        }
    }

    public sealed class GetAssignmentForUpsertQueryValidation : AbstractValidator<GetAssignmentForUpsertQuery>
    {
        public GetAssignmentForUpsertQueryValidation()
        {
            RuleFor(query => query.AssignmentID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AssignmentID must be a valid GUID");
        }
    }
}
