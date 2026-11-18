using FluentValidation;
using System.Application.Services.AcademicPlan.AcademicPlan.Command;
using System.Domain.Enums.AcademicPlan;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Validations
{
    public sealed class NewAcademicPlanCommandValidation : AbstractValidator<NewAcademicPlanCommand>
    {
        public NewAcademicPlanCommandValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x.ProviderInstituteID)
                .Must(IsValidGuid).WithMessage("ProviderInstituteID is required");

            RuleFor(x => x.ScopeType)
                .IsInEnum().WithMessage("ScopeType must be a valid enum value");

            RuleFor(x => x)
                .Must(HaveExactlyOneMatchingScopeReference)
                .WithMessage("Exactly one scope reference matching ScopeType is required");

            RuleFor(x => x)
                .Must(command => command.EffectiveTo == null || command.EffectiveTo >= command.EffectiveFrom)
                .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters");
        }

        private static bool IsValidGuid(string? value) =>
            Guid.TryParse(value, out var id) && id != Guid.Empty;

        private static bool HaveExactlyOneMatchingScopeReference(NewAcademicPlanCommand command)
        {
            var expected = command.ScopeType switch
            {
                enAcademicPlanScopeType.Program => command.ProgramID,
                enAcademicPlanScopeType.InstituteClass => command.InstituteClassID,
                enAcademicPlanScopeType.Stage => command.StageID,
                enAcademicPlanScopeType.StageInfo => command.StageInfoID,
                _ => null
            };

            if (!IsValidGuid(expected))
                return false;

            var references = new[] { command.ProgramID, command.InstituteClassID, command.StageID, command.StageInfoID }
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .ToList();

            return references.Count == 1;
        }
    }

    public sealed class UpdateAcademicPlanCommandValidation : AbstractValidator<UpdateAcademicPlanCommand>
    {
        public UpdateAcademicPlanCommandValidation()
        {
            RuleFor(x => x.AcademicPlanID)
                .Must(IsValidGuid).WithMessage("AcademicPlanID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x)
                .Must(command => command.EffectiveTo == null || command.EffectiveTo >= command.EffectiveFrom)
                .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters");
        }

        private static bool IsValidGuid(string? value) =>
            Guid.TryParse(value, out var id) && id != Guid.Empty;
    }

    public sealed class ActivateAcademicPlanCommandValidation : AbstractValidator<ActivateAcademicPlanCommand>
    {
        public ActivateAcademicPlanCommandValidation()
        {
            RuleFor(x => x.AcademicPlanID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AcademicPlanID is required");
        }
    }

    public sealed class DeleteAcademicPlanCommandValidation : AbstractValidator<DeleteAcademicPlanCommand>
    {
        public DeleteAcademicPlanCommandValidation()
        {
            RuleFor(x => x.AcademicPlanID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("AcademicPlanID is required");
        }
    }
}
