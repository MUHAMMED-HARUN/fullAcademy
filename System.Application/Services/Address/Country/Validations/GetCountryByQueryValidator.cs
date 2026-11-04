using FluentValidation;
using System.Application.Services.Address.Country.Query;

namespace System.Application.Services.Address.Country.Validations
{
    public sealed class GetCountryByQueryValidator : AbstractValidator<GetCountryByQuery>
    {
        public GetCountryByQueryValidator()
        {
            RuleFor(query => query.Param).NotEmpty();
            RuleFor(query => query.ParamType).IsInEnum();
            RuleFor(query => query.Param)
                .Must((query, value) =>
                    query.ParamType != System.Domain.Enums.Address.enCountryParamType.ID ||
                    (Guid.TryParse(value, out var id) && id != Guid.Empty))
                .WithMessage("معرّف الدولة غير صالح");
        }
    }
}
