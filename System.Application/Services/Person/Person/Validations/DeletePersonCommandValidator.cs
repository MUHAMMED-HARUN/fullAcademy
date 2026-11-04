using FluentValidation;
using System.Application.Services.Person.Person.Command;


namespace System.Application.Services.Person.Person.Validations
{
    public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
    {
        public DeletePersonCommandValidator()
        {
            RuleFor(x => x.PersonID)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("Person ID must be a valid, non-empty ID");
        }
    }
}
