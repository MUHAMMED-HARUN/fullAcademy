using FluentValidation;
using System.Application.Services.Assignment.AssignmentScopeInfo.Command;
using System.Application.Services.Assignment.AssignmentScopeInfo.Query;

namespace System.Application.Services.Assignment.AssignmentScopeInfo.Validation
{
    public class GetByAssigmentdValidation : AbstractValidator<GetByAssigmentIDQuery>
    {
        public GetByAssigmentdValidation()
        {
            RuleFor(x => x.AssignmentID)
                .NotEmpty().WithMessage("Assignment ID is required");

        }
    }
}
