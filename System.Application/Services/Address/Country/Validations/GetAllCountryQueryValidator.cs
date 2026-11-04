using FluentValidation;
using System.Application.Services.Address.Country.Query;

namespace System.Application.Services.Address.Country.Validations
{
    public sealed class GetAllCountryQueryValidator : AbstractValidator<GetAllCountryQuery>
    {
        public GetAllCountryQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
            RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
            RuleFor(query => query.Filter).MaximumLength(256);
            RuleFor(query => query.OrderBy).MaximumLength(100);
        }
    }
}
