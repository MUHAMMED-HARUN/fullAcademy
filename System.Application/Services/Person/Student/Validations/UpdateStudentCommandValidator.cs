using FluentValidation;
using System.Application.Services.Person.Student.Command;


namespace System.Application.Services.Person.Student.Validations
{
    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidator()
        {
            RuleFor(x => x.PersonID)
                .Must(value => Guid.TryParseExact(value, "D", out var personID) && personID != Guid.Empty)
                .WithMessage("Person ID must be a valid, non-empty ID");
            RuleFor(x => x.StudentID)
                .Must(value => Guid.TryParseExact(value, "D", out var studentID) && studentID != Guid.Empty)
                .WithMessage("Student ID must be a valid, non-empty ID");
        }
    }
}
