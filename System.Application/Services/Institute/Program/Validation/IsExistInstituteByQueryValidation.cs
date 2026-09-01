using FluentValidation;
using System.Application.Services.Institute.Institute.Query;


namespace System.Application.Services.Institute.Institute.Validator
{
    public class IsExistProgramByQueryValidation : AbstractValidator<IsExistProgramByQuery>
    {
        public IsExistProgramByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
