using FluentValidation;
using System.Application.Services.Assignment.AssignmentScope.Command;
using System.Application.Services.Assignment.AssignmentScope.Query;

namespace System.Application.Services.Assignment.AssignmentScope.Validation
{
    public class GetCanNominateQueryValidation : AbstractValidator<GetCanNominateQuery>
    {
        public GetCanNominateQueryValidation()
        {
            RuleFor(x => x.studentEnrollmentID)
                .NotEmpty().WithMessage("studentEnrollmentID   is required");

        }
    }
}
