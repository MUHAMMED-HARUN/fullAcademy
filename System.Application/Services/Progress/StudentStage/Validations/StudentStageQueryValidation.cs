using FluentValidation;
using System.Application.Services.Progress.StudentStage.Query;

namespace System.Application.Services.Progress.StudentStage.Validations
{
    public sealed class GetStudentStageByQueryValidation : AbstractValidator<GetStudentStageByQuery>
    {
        public GetStudentStageByQueryValidation()
        {
            RuleFor(query => query.Param)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("Param must be a valid GUID");
            RuleFor(query => query.ParamType).IsInEnum();
        }
    }

    public sealed class IsExistStudentStageByQueryValidation : AbstractValidator<IsExistStudentStageByQuery>
    {
        public IsExistStudentStageByQueryValidation()
        {
            RuleFor(query => query.Param)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("Param must be a valid GUID");
            RuleFor(query => query.ParamType).IsInEnum();
        }
    }

    public sealed class GetAllStudentStageQueryValidation : AbstractValidator<GetAllStudentStageQuery>
    {
        public GetAllStudentStageQueryValidation()
        {
            RuleFor(query => query.StudentAcademicEnrollmentID)
                .Must(value => string.IsNullOrWhiteSpace(value) ||
                               (Guid.TryParse(value, out var id) && id != Guid.Empty))
                .WithMessage("StudentAcademicEnrollmentID must be a valid GUID");
        }
    }
}
