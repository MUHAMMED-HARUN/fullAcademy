using FluentValidation;
using System.Application.Services.Progress.StageInfo.Query;



namespace System.Application.Services.Progress.StageInfo.Validations
{
    public class GetStageInfoByQueryValidation : AbstractValidator<GetStageInfoByQuery>
    {
        public GetStageInfoByQueryValidation()
        {
            RuleFor(x => x.Param)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("Param must be a valid, non-empty GUID");
            RuleFor(x => x.ParamType).IsInEnum();
        }
    }
}
