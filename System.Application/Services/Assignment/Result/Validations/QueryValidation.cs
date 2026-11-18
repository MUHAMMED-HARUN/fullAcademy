using FluentValidation;
using System.Application.Services.Assignment.Result.Query;

namespace System.Application.Services.Assignment.Result.Validations
{
    public sealed class GetResultQueryValidation : AbstractValidator<GetResultByQuery>
    {
        public GetResultQueryValidation()
        {
            RuleFor(query => query.ID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("ResultID must be a valid GUID");
        }
    }

    public sealed class GetResultForUpsertQueryValidation : AbstractValidator<GetResultForUpsertQuery>
    {
        public GetResultForUpsertQueryValidation()
        {
            RuleFor(query => query.ResultID)
                .NotEmpty()
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("ResultID must be a valid GUID");
        }
    }

    public sealed class GetAllResultQueryValidation : AbstractValidator<GetAllResultQuery>
    {
        public GetAllResultQueryValidation()
        {
            RuleFor(query => query.AssignmentID)
                .Must(value => string.IsNullOrWhiteSpace(value) ||
                               (Guid.TryParse(value, out var id) && id != Guid.Empty))
                .WithMessage("AssignmentID must be a valid GUID");
        }
    }
}
