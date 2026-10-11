using FluentValidation;
using System.Application.Services.Attendance.Attendance.Command;

namespace System.Application.Services.Attendance.Attendance.Validation
{
    public class NewAttendanceCommandCommandValidation : AbstractValidator<NewAttendanceCommand>
    {
        public NewAttendanceCommandCommandValidation()
        {
            RuleFor(x => x.Attendances)
           .NotEmpty().WithMessage("Attendances are required");

            RuleForEach(x => x.Attendances).ChildRules(attendance =>
            {
                attendance.RuleFor(a => a.AttendeeType)
                    .IsInEnum().WithMessage("AttendeeType must be a valid enum value");
                attendance.RuleFor(a => a.Status)
                    .IsInEnum().WithMessage("Status must be a valid enum value");
            });
        }
    }
}
