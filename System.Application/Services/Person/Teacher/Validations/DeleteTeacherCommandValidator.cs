using FluentValidation;
using System.Application.Services.Person.Teacher.Command;


namespace System.Application.Services.Person.Teacher.Validations
{
    public class DeleteTeacherCommandValidator : AbstractValidator<DeleteTeacherCommand>
    {
        public DeleteTeacherCommandValidator()
        {
            RuleFor(x => x.TeacherID)
                .Must(value => Guid.TryParseExact(value, "D", out var id) && id != Guid.Empty)
                .WithMessage("Teacher ID must be a valid, non-empty ID");
        }
    }
}
