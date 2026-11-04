using FluentValidation;
using System.Application.Services.Address.Neighborhood.Query;

namespace System.Application.Services.Address.Neighborhood.Validations
{
    public sealed class GetAllNeighborhoodQueryValidator
        : AbstractValidator<GetAllNeighborhoodQuery>
    {
        public GetAllNeighborhoodQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThanOrEqualTo(1);
            RuleFor(query => query.PageSize).InclusiveBetween(1, 100);
            RuleFor(query => query.Filter).MaximumLength(256);
            RuleFor(query => query.OrderBy).MaximumLength(100);
            RuleFor(query => query.DistrictID)
                .Must(districtID => districtID is null || districtID != Guid.Empty)
                .WithMessage("معرّف المنطقة غير صالح");
        }
    }
}
