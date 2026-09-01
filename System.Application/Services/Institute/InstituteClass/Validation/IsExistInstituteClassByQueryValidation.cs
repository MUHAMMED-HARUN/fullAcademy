using FluentValidation;
using System.Application.Services.Institute.Institute.Query;
using System.Application.Services.Institute.InstituteClass.Query;


namespace System.Application.Services.Institute.Institute.Validator
{
    public class IsExistInstituteClassByQueryValidation : AbstractValidator<IsExistInstituteClassByQuery>
    {
        public IsExistInstituteClassByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).NotEmpty().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
