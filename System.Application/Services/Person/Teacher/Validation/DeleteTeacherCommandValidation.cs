using FluentValidation;
using System.Application.Services.Person.Person.Command;
using System.Application.Services.Person.Teacher.Command;


namespace System.Application.Services.Person.Teacher.Validation
{
    public class DeleteStudenCommandValidation : AbstractValidator<DeleteTeacherCommand>
    {
        public DeleteStudenCommandValidation()
        {
            RuleFor(x => x.TeacherID).NotEmpty().WithMessage("Teacher id MustBe Not Empty");
        }
    }
}
