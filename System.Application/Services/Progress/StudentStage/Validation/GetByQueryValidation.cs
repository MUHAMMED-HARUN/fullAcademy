using FluentValidation;
using System.Application.Services.Progress.StudentStage.Query;



namespace System.Application.Services.Progress.StudentStage.Validator
{
    public class GetStudentStageByQueryValidation : AbstractValidator<GetStudentStageByQuery>
    {
        public GetStudentStageByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
