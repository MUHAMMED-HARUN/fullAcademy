using FluentValidation;
using System.Application.Services.Person.Person.Command;
using System.Application.Services.Person.Student.Command;


namespace System.Application.Services.Person.Student.Validation
{
    public class DeleteStudenCommandValidation : AbstractValidator<DeleteStudentCommand>
    {
        public DeleteStudenCommandValidation()
        {
            RuleFor(x => x.StudentID).NotEmpty().WithMessage("Student id MustBe Not Empty");
        }
    }
}
