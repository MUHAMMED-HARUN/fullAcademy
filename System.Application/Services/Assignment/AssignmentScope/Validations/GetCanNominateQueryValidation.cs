using FluentValidation;
using System.Application.Services.Assignment.AssignmentScope.Query;

namespace System.Application.Services.Assignment.AssignmentScope.Validations
{
    public class GetCanNominateQueryValidation : AbstractValidator<GetCanNominateQuery>
    {
        public GetCanNominateQueryValidation()
        {
            RuleFor(x => x.StudentAcademicEnrollmentID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentAcademicEnrollmentID must be a valid GUID");

        }
    }
}
