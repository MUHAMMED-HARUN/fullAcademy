using FluentValidation;
using System.Application.Services.Progress.Session.Command;


namespace System.Application.Services.Progress.Session.Validator
{
    public class DeleteSessionCommandValidation : AbstractValidator<DeleteSessionCommand>
    {
        public DeleteSessionCommandValidation()
        {
            RuleFor(x => x.SessionID).NotEmpty().WithMessage("Session id MustBe Not Empty");
        }
    }
}
