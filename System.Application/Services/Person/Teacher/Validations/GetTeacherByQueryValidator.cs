using FluentValidation;
using System.Application.Services.Person.Teacher.Query;


namespace System.Application.Services.Person.Teacher.Validations
{
    public class GetTeacherByQueryValidator : AbstractValidator<GetTeacherByQuery>
    {
        public GetTeacherByQueryValidator()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType is invalid");
            RuleFor(x => x.Param)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .When(x => x.ParamType is System.Domain.Enums.Person.enTeacherParamType.ID or System.Domain.Enums.Person.enTeacherParamType.PersonID)
                .WithMessage("Teacher/Person ID must be a valid, non-empty ID");
        }
    }
}
