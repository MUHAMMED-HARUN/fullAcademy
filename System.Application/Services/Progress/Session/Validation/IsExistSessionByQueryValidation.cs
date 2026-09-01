using FluentValidation;
using System.Application.Services.Progress.Session.Query;


namespace System.Application.Services.Progress.Session.Validator
{
    public class IsExistSessionByQueryValidation : AbstractValidator<IsExistSessionByQuery>
    {
        public IsExistSessionByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
