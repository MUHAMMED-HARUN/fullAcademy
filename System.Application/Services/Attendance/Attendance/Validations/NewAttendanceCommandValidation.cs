using FluentValidation;
using System.Application.Services.Attendance.Attendance.Command;
using System.Application.Services.Attendance.Attendance.Query;
using System.Domain.Enums.Attendance;

namespace System.Application.Services.Attendance.Attendance.Validations
{
    public sealed class NewAttendanceCommandValidation : AbstractValidator<NewAttendanceCommand>
    {
        public NewAttendanceCommandValidation()
        {
            RuleFor(x => x.Date)
                .NotEqual(default(DateOnly)).WithMessage("Date is required");
            RuleFor(x => x.Attendances)
                .NotEmpty().WithMessage("At least one group member is required");
            RuleFor(x => x.Attendances)
                .Must(items => items == null || items.Count <= 500)
                .WithMessage("An attendance batch cannot exceed 500 group members");
            RuleFor(x => x.Attendances)
                .Must(items => items != null && items.All(item => item != null &&
                    Guid.TryParse(item.EntityID, out var id) && id != Guid.Empty &&
                    Guid.TryParse(item.ContextID, out var groupID) && groupID != Guid.Empty &&
                    item.AttendeeType == enAttendeeType.Student &&
                    item.ContextType == enContextType.Group &&
                    Enum.IsDefined(typeof(AttendanceStatus), item.Status)))
                .WithMessage("Attendance rows must identify a student group membership and its group");
            RuleFor(x => x.Attendances)
                .Must(items => items == null || items.All(item => item != null) &&
                    items.Select(item => item.EntityID).Distinct().Count() == items.Count)
                .WithMessage("A group membership can appear only once in an attendance batch");
            RuleFor(x => x.Attendances)
                .Must(items => items == null || items.All(item => item != null) &&
                    items.Select(item => item.ContextID).Distinct().Count() <= 1)
                .WithMessage("An attendance batch must belong to one group");
        }
    }

    public sealed class GetGroupAttendanceQueryValidation : AbstractValidator<GetGroupAttendanceQuery>
    {
        public GetGroupAttendanceQueryValidation()
        {
            RuleFor(x => x.GroupID)
                .Must(value => Guid.TryParse(value, out var id) && id != Guid.Empty)
                .WithMessage("GroupID must be a valid GUID");
            RuleFor(x => x.Date)
                .NotEqual(default(DateOnly)).WithMessage("Date is required");
        }
    }
}
