using FluentValidation;
using System.Application.Services.Progress.Session.Command;


namespace System.Application.Services.Progress.Session.Validator
{
    public class NewSessionCommandValidation : AbstractValidator<NewSessionCommand>
    {

        public NewSessionCommandValidation()
        {
            RuleFor(x => x.SessionName).NotEmpty().WithMessage("Name MustBe Not Empty");
            RuleFor(x => x.SessionNote).NotEmpty().WithMessage("Note MustBe Not Empty");
            RuleFor(x => x.SessionType).IsInEnum().WithMessage("Note MustBe Not Empty");
        }
    }

}
