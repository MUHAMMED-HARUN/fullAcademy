using FluentValidation;
using System.Application.Services.Progress.StageInfo.Query;



namespace System.Application.Services.Progress.StageInfo.Validations
{
    public class GetStudentStageInfoQueryValidation : AbstractValidator<GetStudentStageInfoQuery>
    {
        public GetStudentStageInfoQueryValidation()
        {
            RuleFor(x => x.StudentAcademicEnrollmentID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("StudentAcademicEnrollmentID must be a valid, non-empty GUID");
        }
    }
}
