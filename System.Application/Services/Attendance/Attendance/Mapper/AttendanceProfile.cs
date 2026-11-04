using AutoMapper;
using System.Application.Common;
using System.Application.Services.Assignment.Assignment.Command;
using System.Application.Services.Attendance.Attendance.Command;
using System.Application.Services.Enrollment.StudentEnrollment.Command;
using System.Domain.Entities;
using System.Domain.Entities.Attendance;

namespace System.Application.Services.Attendance.Attendance.Mapper
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            CreateMap<AttendanceCommandDTO, clsAttendance>()
                .ConstructUsing(src => new clsAttendance(
                    clsUtil.ToGuid(src.EntityID),
                    src.AttendeeType,
                    DateOnly.FromDateTime(DateTime.Now),
                    src.Status,
                    null,
                    null,
                    !string.IsNullOrEmpty(src.ContextID) ? clsUtil.ToGuid(src.ContextID) : null,
                    src.ContextType,
                    null,
                    null
                ))
                .ForAllMembers(opt => opt.Ignore());
        }
    }
}
