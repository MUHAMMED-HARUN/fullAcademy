using FluentValidation;
using System.Application.Services.Institute.InstituteClass.Command;

namespace System.Application.Services.Institute.InstituteClass.Validations
{
    public sealed class NewInstituteClassByRangeCommandValidator
        : AbstractValidator<NewInstituteClassByRangeCommand>
    {
        public NewInstituteClassByRangeCommandValidator()
        {
            RuleFor(command => command.InstituteID)
                .Must(BeNonEmptyGuid)
                .WithMessage("معرّف المعهد غير صالح");

            RuleFor(command => command.ClassIDs)
                .NotNull()
                .NotEmpty()
                .WithMessage("يجب اختيار صف واحد على الأقل");

            RuleForEach(command => command.ClassIDs)
                .Must(BeNonEmptyGuid)
                .WithMessage("أحد معرّفات الصفوف غير صالح");
        }

        private static bool BeNonEmptyGuid(string? value) =>
            Guid.TryParse(value, out var id) && id != Guid.Empty;
    }
}
