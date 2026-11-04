using FluentValidation;
using System.Application.Services.Address.Neighborhood.Query;
using System.Domain.Enums.Address;

namespace System.Application.Services.Address.Neighborhood.Validations
{
    public sealed class GetNeighborhoodByQueryValidator : AbstractValidator<GetNeighborhoodByQuery>
    {
        public GetNeighborhoodByQueryValidator()
        {
            RuleFor(query => query.Param).NotEmpty();
            RuleFor(query => query.ParamType).IsInEnum();
            RuleFor(query => query.Param)
                .Must((query, value) =>
                    query.ParamType != enNeighborhoodParamType.ID ||
                    (Guid.TryParse(value, out var id) && id != Guid.Empty))
                .WithMessage("معرّف الشارع غير صالح");
        }
    }
}
