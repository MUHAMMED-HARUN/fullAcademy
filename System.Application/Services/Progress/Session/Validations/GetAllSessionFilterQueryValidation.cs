using FluentValidation;
using System.Application.Services.Progress.Session.Query;

namespace System.Application.Services.Progress.Session.Validations
{
    public sealed class GetAllSessionFilterQueryValidation : AbstractValidator<GetAllSessionFilterQuery>
    {
        public GetAllSessionFilterQueryValidation()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
            RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
        }
    }
}
