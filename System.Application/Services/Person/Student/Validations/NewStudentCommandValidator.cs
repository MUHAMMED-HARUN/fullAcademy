using FluentValidation;

using System.Application.Services.Person.Student.Command;


namespace System.Application.Services.Person.Student.Validations
{
    public class NewStudentCommandValidator : AbstractValidator<NewStudentCommand>
    {
        public NewStudentCommandValidator()
        {
            RuleFor(x => x.PersonID)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("Person ID must be a valid, non-empty ID");
        }
    }
}
