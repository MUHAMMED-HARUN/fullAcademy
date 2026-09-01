using FluentValidation;
using System;
using System.Application.Services.Enrollment.StudentEnrollment.Command;

namespace System.Application.Services.Enrollment.StudentEnrollment.Validation
{
    public class NewStudentEnrollmentCommandValidation : AbstractValidator<NewStudentEnrollmentCommand>
    {
        public NewStudentEnrollmentCommandValidation()
        {
            RuleFor(x => x.StudentID).NotEmpty().WithMessage("StudentID must not be empty");
            RuleFor(x => x.GroupID).NotEmpty().WithMessage("GroupID must not be empty");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date must not be empty")
                                .Must(date => date != default).WithMessage("A valid Date is required");
        }
    }
}
