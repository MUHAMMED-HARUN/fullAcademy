using FluentValidation;
using System.Application.Services.Progress.StageInfo.Query;


namespace System.Application.Services.Progress.StageInfo.Validator
{
    public class IsExistStageInfoByQueryValidation : AbstractValidator<IsExistStageInfoByQuery>
    {
        public IsExistStageInfoByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
