using FluentValidation;
using System.Application.Services.Address.District.Query;

namespace System.Application.Services.Address.District.Validations
{
    public sealed class GetAllDistrictQueryValidator : AbstractValidator<GetAllDistrictQuery>
    {
        public GetAllDistrictQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
            RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
            RuleFor(query => query.Filter).MaximumLength(256);
            RuleFor(query => query.OrderBy).MaximumLength(100);
            RuleFor(query => query.CityID)
                .Must(cityID => cityID is null || cityID != Guid.Empty)
                .WithMessage("معرّف المدينة غير صالح");
        }
    }
}
