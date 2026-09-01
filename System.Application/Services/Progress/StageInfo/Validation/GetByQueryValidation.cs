using FluentValidation;
using System.Application.Services.Progress.StageInfo.Query;



namespace System.Application.Services.Progress.StageInfo.Validator
{
    public class GetStageInfoByQueryValidation : AbstractValidator<GetStageInfoByQuery>
    {
        public GetStageInfoByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
