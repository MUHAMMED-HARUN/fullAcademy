using FluentValidation;
using System.Application.Services.Enrollment.TeachingAssignment.Command;

namespace System.Application.Services.Enrollment.TeachingAssignment.Validation
{
    public class DeleteTeachingAssignmentCommandValidation : AbstractValidator<DeleteTeachingAssignmentCommand>
    {
        public DeleteTeachingAssignmentCommandValidation()
        {
            RuleFor(x => x.TeachingAssignmentID).NotEmpty().WithMessage("EnrollmentID must not be empty");
        }
    }
}
