using FluentValidation;

using System.Application.Services.Person.Student.Command;


namespace System.Application.Services.Person.Student.Validation
{
    public class NewStudentCommandValidation :AbstractValidator<NewStudentCommand>
    {
        public NewStudentCommandValidation()
        {
            RuleFor(x => x.PersonID).NotEmpty().WithMessage("Person ID MustBe Not Empty");
          

        }
    }
}
