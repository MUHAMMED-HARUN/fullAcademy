using FluentValidation;
using System.Application.Services.Person.Student.Query;


namespace System.Application.Services.Person.Student.Validation
{
    public class GetStudentByQueryValidation : AbstractValidator<GetStudentByQuery>
    {
        public GetStudentByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
