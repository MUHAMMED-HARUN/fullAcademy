using FluentValidation;
using System.Application.Services.Person.Teacher.Command;


namespace System.Application.Services.Person.Teacher.Validations
{
    public class UpdateTeacherCommandValidator : AbstractValidator<UpdateTeacherCommand>
    {
        public UpdateTeacherCommandValidator()
        {
            RuleFor(x => x.PersonID)
                .Must(value => Guid.TryParseExact(value, "D", out var personID) && personID != Guid.Empty)
                .WithMessage("Person ID must be a valid, non-empty ID");
            RuleFor(x => x.TeacherID)
                .Must(value => Guid.TryParseExact(value, "D", out var teacherID) && teacherID != Guid.Empty)
                .WithMessage("Teacher ID must be a valid, non-empty ID");
        }
    }
}
