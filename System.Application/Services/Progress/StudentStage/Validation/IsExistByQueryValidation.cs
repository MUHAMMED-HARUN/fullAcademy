using FluentValidation;
using System.Application.Services.Progress.StudentStage.Query;


namespace System.Application.Services.Progress.StudentStage.Validator
{
    public class IsExistStudentStageByQueryValidation : AbstractValidator<IsExistStudentStageByQuery>
    {
        public IsExistStudentStageByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
