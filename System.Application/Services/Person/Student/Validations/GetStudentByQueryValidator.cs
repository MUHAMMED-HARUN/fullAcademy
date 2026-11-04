using FluentValidation;
using System.Application.Services.Person.Student.Query;


namespace System.Application.Services.Person.Student.Validations
{
    public class GetStudentByQueryValidator : AbstractValidator<GetStudentByQuery>
    {
        public GetStudentByQueryValidator()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType is invalid");
            RuleFor(x => x.Param)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .When(x => x.ParamType is System.Domain.Enums.Person.enStudentParamType.ID or System.Domain.Enums.Person.enStudentParamType.PersonID)
                .WithMessage("Student/Person ID must be a valid, non-empty ID");
        }
    }
}
