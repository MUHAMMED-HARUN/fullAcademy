using FluentValidation;
using System;
using System.Application.Services.Enrollment.TeachingAssignment.Command;

namespace System.Application.Services.Enrollment.TeachingAssignment.Validation
{
    public class NewTeachingAssignmentCommandValidation : AbstractValidator<NewTeachingAssignmentCommand>
    {
        public NewTeachingAssignmentCommandValidation()
        {
            RuleFor(x => x.TeacherID).NotEmpty().WithMessage("TeacherID must not be empty");
            RuleFor(x => x.GroupID).NotEmpty().WithMessage("GroupID must not be empty");
            RuleFor(x => x.SubjectID).NotEmpty().WithMessage("SubjectID must not be empty");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date must not be empty")
                                .Must(date => date != default).WithMessage("A valid Date is required");
        }
    }
}
