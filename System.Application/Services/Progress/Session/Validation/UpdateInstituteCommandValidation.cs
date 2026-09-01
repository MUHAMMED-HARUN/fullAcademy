using FluentValidation;
using System.Application.Services.Progress.Session.Command;


namespace System.Application.Services.Person.Person.Validation
{
    public class UpdateSessionCommandValidation : AbstractValidator<UpdateSessionCommand>
    {
        public UpdateSessionCommandValidation()
        {
            RuleFor(x => x.SessionID).NotEmpty().WithMessage("Person ID MustBe Not Empty");
            RuleFor(x => x.SessionName).NotEmpty().WithMessage("Name MustBe Not Empty");
            RuleFor(x => x.SessionType).IsInEnum().WithMessage("Note MustBe Not Empty");
        }
    }
}
