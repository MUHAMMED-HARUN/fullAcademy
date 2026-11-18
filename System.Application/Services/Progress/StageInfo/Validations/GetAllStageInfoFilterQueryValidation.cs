using FluentValidation;
using System.Application.Services.Progress.StageInfo.Query;

namespace System.Application.Services.Progress.StageInfo.Validations
{
    public sealed class GetAllStageInfoFilterQueryValidation
        : AbstractValidator<GetAllStageInfoFilterQuery>
    {
        public GetAllStageInfoFilterQueryValidation()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
            RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
            RuleFor(query => query.StageID)
                .Must(value => string.IsNullOrWhiteSpace(value) || Guid.TryParse(value, out var id) && id != Guid.Empty);
        }
    }
}
