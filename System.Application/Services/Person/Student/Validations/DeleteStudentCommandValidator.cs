using FluentValidation;
using System.Application.Services.Person.Student.Command;


namespace System.Application.Services.Person.Student.Validations
{
    public class DeleteStudentCommandValidator : AbstractValidator<DeleteStudentCommand>
    {
        public DeleteStudentCommandValidator()
        {
            RuleFor(x => x.StudentID)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("Student ID must be a valid, non-empty ID");
        }
    }
}
