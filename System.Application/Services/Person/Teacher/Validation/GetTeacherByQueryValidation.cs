using FluentValidation;
using System.Application.Services.Person.Teacher.Query;


namespace System.Application.Services.Person.Teacher.Validation
{
    public class GetTeacherByQueryValidation : AbstractValidator<GetTeacherByQuery>
    {
        public GetTeacherByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
