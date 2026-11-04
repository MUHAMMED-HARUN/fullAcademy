using FluentValidation;
using System.Application.Services.Progress.Stage.Query;


namespace System.Application.Services.Progress.Stage.Validations
{
    public class IsExistStageByQueryValidation : AbstractValidator<IsExistStageByQuery>
    {
        public IsExistStageByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType is invalid");
        }
    }
}
