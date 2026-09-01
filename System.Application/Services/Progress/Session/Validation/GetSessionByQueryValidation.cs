using FluentValidation;
using System.Application.Services.Progress.Session.Query;



namespace System.Application.Services.Progress.Session.Validator
{
    public class GetSessionByQueryValidation : AbstractValidator<GetSessionByQuery>
    {
        public GetSessionByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
