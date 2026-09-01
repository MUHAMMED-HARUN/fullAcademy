using FluentValidation;
using System.Application.Services.Institute.Institute.Query;


namespace System.Application.Services.Institute.Institute.Validator
{
    public class GetInstituteByQueryValidation : AbstractValidator<GetInstituteByQuery>
    {
        public GetInstituteByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param MustBe Not Empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType MustBe Not Empty");
        }
    }
}
