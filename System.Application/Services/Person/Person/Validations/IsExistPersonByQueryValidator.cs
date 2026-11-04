using FluentValidation;
using System.Application.Services.Person.Person.Query;


namespace System.Application.Services.Person.Person.Validations
{
    public class IsExistPersonByQueryValidator : AbstractValidator<IsExistPersonByQuery>
    {
        public IsExistPersonByQueryValidator()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType is invalid");
            RuleFor(x => x.Param)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .When(x => x.ParamType == System.Domain.Enums.Person.enPersonParamType.ID)
                .WithMessage("Person ID must be a valid, non-empty ID");
        }
    }
}
