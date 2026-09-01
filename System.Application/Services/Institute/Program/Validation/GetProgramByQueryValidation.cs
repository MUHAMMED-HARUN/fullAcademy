using FluentValidation;
using System.Application.Services.Institute.Institute.Query;
using System.Domain.Enums.Institute;


namespace System.Application.Services.Institute.Institute.Validator
{
    public class GetProgramByQueryValidation : AbstractValidator<GetProgramByQuery>
    {
        public GetProgramByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
