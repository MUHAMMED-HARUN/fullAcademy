using FluentValidation;
using System.Application.Services.Institute.Group.Query;

namespace System.Application.Services.Institute.Group.Validation
{
    public class IsExistGroupByQueryValidation : AbstractValidator<IsExistGroupByQuery>
    {
        public IsExistGroupByQueryValidation()
        {
            RuleFor(x => x.Param).NotEmpty().WithMessage("Param must not be empty");
            RuleFor(x => x.ParamType).IsInEnum().WithMessage("ParamType must not be empty");

        }
    }
}
