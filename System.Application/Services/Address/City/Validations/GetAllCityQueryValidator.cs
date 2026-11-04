using FluentValidation;
using System.Application.Services.Address.City.Query;

namespace System.Application.Services.Address.City.Validations
{
    public sealed class GetAllCityQueryValidator : AbstractValidator<GetAllCityQuery>
    {
        public GetAllCityQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
            RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
            RuleFor(query => query.Filter).MaximumLength(256);
            RuleFor(query => query.OrderBy).MaximumLength(100);
        }
    }
}
