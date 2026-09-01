using FluentValidation;
using System.Application.Services.Person.Teacher.Command;


namespace System.Application.Services.Person.Teacher.Validation
{
    public class UpdateTeacherCommandValidation :AbstractValidator<UpdateTeacherCommand>
    {
        public UpdateTeacherCommandValidation()
        {
            RuleFor(x => x.PersonID).NotEmpty().WithMessage("Person ID MustBe Not Empty");
            RuleFor(x => x.TeacherID).NotEmpty().WithMessage("Teacher ID MustBe Not Empty");
          

        }
    }
}
