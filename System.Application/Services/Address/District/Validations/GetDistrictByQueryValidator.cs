using FluentValidation;
using System.Application.Services.Address.District.Query;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.District.Validations
{
    public sealed class GetDistrictByQueryValidator : AbstractValidator<GetDistrictByQuery>
    {
        public GetDistrictByQueryValidator()
        {
            RuleFor(query => query.Param).NotEmpty();
            RuleFor(query => query.ParamType).IsInEnum();
            RuleFor(query => query.Param)
                .Must((query, value) =>
                    query.ParamType != enDistrictParamType.ID ||
                    (Guid.TryParse(value, out var id) && id != Guid.Empty))
                .WithMessage("معرّف المنطقة غير صالح");
        }
    }
}
