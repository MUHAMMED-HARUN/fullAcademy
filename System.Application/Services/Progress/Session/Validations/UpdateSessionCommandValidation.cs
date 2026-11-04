using FluentValidation;
using System.Application.Services.Progress.Session.Command;


namespace System.Application.Services.Progress.Session.Validations
{
    public class UpdateSessionCommandValidation : AbstractValidator<UpdateSessionCommand>
    {
        public UpdateSessionCommandValidation()
        {
            RuleFor(x => x.SessionID).NotEmpty().WithMessage("Person ID MustBe Not Empty");
            RuleFor(x => x.SessionName).NotEmpty().WithMessage("Name MustBe Not Empty");
            RuleFor(x => x.SessionName).MaximumLength(100);
            RuleFor(x => x.SessionNote).MaximumLength(500);
            RuleFor(x => x.SessionType).IsInEnum().WithMessage("Note MustBe Not Empty");
            RuleFor(x => x.ParentSessionID)
                .Must(value => string.IsNullOrWhiteSpace(value) || Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("Parent Session ID must be a valid ID");
        }
    }
}
