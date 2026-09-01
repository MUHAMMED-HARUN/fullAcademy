using FluentValidation;
using System.Application.Services.Progress.StageInfo.Query;



namespace System.Application.Services.Progress.StageInfo.Validator
{
    public class GetStudentStageInfoQueryValidation : AbstractValidator<GetStudentStageInfoQuery>
    {
        public GetStudentStageInfoQueryValidation()
        {
            RuleFor(x => x.enrollmentID).NotEmpty().WithMessage("enrollmentID MustBe Not Empty");
        }
    }
}
