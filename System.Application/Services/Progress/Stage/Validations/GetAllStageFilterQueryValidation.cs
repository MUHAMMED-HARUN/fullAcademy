using FluentValidation;
using System.Application.Services.Progress.Stage.Query;

namespace System.Application.Services.Progress.Stage.Validations
{
    public sealed class GetAllStageFilterQueryValidation : AbstractValidator<GetAllStageFilterQuery>
    {
        public GetAllStageFilterQueryValidation()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
            RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
            RuleFor(query => query.ClassID)
                .Must(value => string.IsNullOrWhiteSpace(value) || Guid.TryParse(value, out var id) && id != Guid.Empty);
            RuleFor(query => query.ExcludeStageID)
                .Must(value => string.IsNullOrWhiteSpace(value) || Guid.TryParse(value, out var id) && id != Guid.Empty);
        }
    }
}
