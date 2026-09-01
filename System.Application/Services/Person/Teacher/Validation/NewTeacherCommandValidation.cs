using FluentValidation;

using System.Application.Services.Person.Teacher.Command;


namespace System.Application.Services.Person.Teacher.Validation
{
    public class NewTeacherCommandValidation :AbstractValidator<NewTeacherCommand>
    {
        public NewTeacherCommandValidation()
        {
            RuleFor(x => x.PersonID).NotEmpty().WithMessage("Person ID MustBe Not Empty");
          

        }
    }
}
