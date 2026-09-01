using FluentValidation;
using System.Application.Services.Person.Person.Query;


namespace System.Application.Services.Person.Person.Validation
{
    public class GetPersonByQueryValidation : AbstractValidator<GetPersonByQuery>
    {
        public GetPersonByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
