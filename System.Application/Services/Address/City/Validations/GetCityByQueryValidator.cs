using FluentValidation;
using System.Application.Services.Address.City.Query;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.City.Validations
{
    public sealed class GetCityByQueryValidator : AbstractValidator<GetCityByQuery>
    {
        public GetCityByQueryValidator()
        {
            RuleFor(query => query.Param).NotEmpty();
            RuleFor(query => query.ParamType).IsInEnum();
            RuleFor(query => query.Param)
                .Must((query, value) =>
                    query.ParamType != enCityParamType.ID ||
                    (Guid.TryParse(value, out var id) && id != Guid.Empty))
                .WithMessage("معرّف المدينة غير صالح");
        }
    }
}
