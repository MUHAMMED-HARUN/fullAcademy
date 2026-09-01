using FluentValidation;
using System.Application.Services.Institute.Class.Query;


namespace System.Application.Services.Institute.Class.Validator
{
    public class IsExistClassByQueryValidation : AbstractValidator<IsExistClassByQuery>
    {
        public IsExistClassByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
