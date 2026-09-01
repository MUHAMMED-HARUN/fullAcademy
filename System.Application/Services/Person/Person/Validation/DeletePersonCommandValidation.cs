using FluentValidation;
using System.Application.Services.Address.City.Query;
using System.Application.Services.Person.Person.Command;


namespace System.Application.Services.Person.Person.Validation
{
    public class DeletePersonCommandValidation : AbstractValidator<DeletePersonCommand>
    {
        public DeletePersonCommandValidation()
        {
            RuleFor(x => x.PersonID).NotEmpty().WithMessage("Person id MustBe Not Empty");
        }
    }
}
