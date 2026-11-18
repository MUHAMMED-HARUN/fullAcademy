using FluentValidation;
using System.Application.Services.Enrollment.StudentEnrollment.Command;

namespace System.Application.Services.Enrollment.StudentEnrollment.Validations
{
    public sealed class EnrollByRangeCommandValidation : AbstractValidator<EnrollByRangeCommand>
    {
        public EnrollByRangeCommandValidation()
        {
            RuleFor(x => x.GroupID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("GroupID must be a valid, non-empty GUID");
            RuleFor(x => x.StudentAcademicEnrollmentIDs)
                .NotEmpty().WithMessage("At least one StudentAcademicEnrollmentID is required")
                .Must(ids => ids.All(value => Guid.TryParse(value, out var id) && id != Guid.Empty))
                .WithMessage("All StudentAcademicEnrollmentIDs must be valid GUIDs");
            RuleFor(x => x.Date).NotEqual(default(DateOnly)).WithMessage("A valid Date is required");
        }
    }
}
