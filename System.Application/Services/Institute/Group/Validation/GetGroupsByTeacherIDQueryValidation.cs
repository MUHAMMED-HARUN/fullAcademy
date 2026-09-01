using FluentValidation;
using System.Application.Services.Institute.Group.Query;

namespace System.Application.Services.Institute.Group.Validation
{
    public class GetGroupsByTeacherIDQueryValidation : AbstractValidator<GetGroupsByTeacherIDQuery>
    {
        public GetGroupsByTeacherIDQueryValidation()
        {
            RuleFor(x => x.TeacherID).NotEmpty().WithMessage("TeacherID must not be empty");
        }
    }
}
