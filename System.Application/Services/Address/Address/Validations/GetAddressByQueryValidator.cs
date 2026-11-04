using FluentValidation;
using System.Application.Services.Address.Address.Query;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.Address.Validations
{
    public sealed class GetAddressByQueryValidator : AbstractValidator<GetAddressByQuery>
    {
        public GetAddressByQueryValidator()
        {
            RuleFor(query => query.Param).NotEmpty();
            RuleFor(query => query.ParamType).IsInEnum();
            RuleFor(query => query.Param)
                .Must((query, value) =>
                    Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("المعرّف غير صالح");
        }
    }
}
