using FluentValidation;
using System.Application.Services.Person.Student.Command;


namespace System.Application.Services.Person.Student.Validation
{
    public class UpdateStudentCommandValidation :AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidation()
        {
            RuleFor(x => x.PersonID).NotEmpty().WithMessage("Person ID MustBe Not Empty");
            RuleFor(x => x.StudentID).NotEmpty().WithMessage("Student ID MustBe Not Empty");
          

        }
    }
}
