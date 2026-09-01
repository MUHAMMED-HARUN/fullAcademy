using FluentValidation;
using System.Application.Services.Person.Student.Query;


namespace System.Application.Services.Person.Student.Validation
{
    public class IsExistStudentByQueryValidation : AbstractValidator<IsExistStudentByQuery>
    {
        public IsExistStudentByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
