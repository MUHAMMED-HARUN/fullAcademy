using FluentValidation;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.Command;

namespace System.Application.Services.Enrollment.StudentAcademicEnrollment.Validations
{
    public sealed class EnrollStudentAcademicPlanCommandValidation : AbstractValidator<EnrollStudentAcademicPlanCommand>
    {
        public EnrollStudentAcademicPlanCommandValidation()
        {
            RuleFor(x => x.StudentID).Must(IsGuid).WithMessage("StudentID is required");
            RuleFor(x => x.AcademicPlanID).Must(IsGuid).WithMessage("AcademicPlanID is required");
            RuleFor(x => x.EnrollmentDate).NotEqual(default(DateOnly)).WithMessage("EnrollmentDate is required");
            RuleFor(x => x.ValidFrom).NotEqual(default(DateOnly)).WithMessage("ValidFrom is required");
            RuleFor(x => x).Must(x => x.ValidTo == null || x.ValidTo >= x.ValidFrom)
                .WithMessage("ValidTo must be on or after ValidFrom");
            RuleFor(x => x).Must(x => x.EnrollmentDate >= x.ValidFrom && (x.ValidTo == null || x.EnrollmentDate <= x.ValidTo))
                .WithMessage("EnrollmentDate must be within the validity period");
        }

        private static bool IsGuid(string? value) => Guid.TryParse(value, out var id) && id != Guid.Empty;
    }
}
