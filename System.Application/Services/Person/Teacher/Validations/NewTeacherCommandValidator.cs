using FluentValidation;

using System.Application.Services.Person.Teacher.Command;


namespace System.Application.Services.Person.Teacher.Validations
{
    public class NewTeacherCommandValidator : AbstractValidator<NewTeacherCommand>
    {
        public NewTeacherCommandValidator()
        {
            RuleFor(x => x.PersonID)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("Person ID must be a valid, non-empty ID");
        }
    }
}
