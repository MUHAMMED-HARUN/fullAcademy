using System;
using System.Domain.Entities.BaseClass;
using System.Domain.Enums.Attendance;
using System.Domain.Exceptions;
using System.Linq;
using System.Linq.Expressions;

namespace System.Domain.Entities.Attendance
{
    public class clsAttendance : clsBaseClass
    {
        public static Expression<Func<clsAttendance, bool>> GetByAttendees_Date(IEnumerable<Guid> attendeeIds, DateOnly date) =>
          a => attendeeIds.Contains(a.AttendeeID) && a.Date == date && a.IsActive == true;
        public static Expression<Func<clsAttendance, bool>> GetByAttendees_Date_Context(
            IEnumerable<Guid> attendeeIds,
            DateOnly date,
            IEnumerable<Guid> contextId =null,
            IEnumerable<enContextType> contextType = null)
        {
            return a => attendeeIds.Contains(a.AttendeeID)
                        && a.Date == date
                        && a.IsActive == true
                        && (contextId == null || !contextId.Any() || contextId.Contains(a.ContextID.Value))
                        && (contextType == null || !contextType.Any() || contextType.Contains(a.ContextType.Value));
        }
        public Guid AttendeeID { get; private set; }
        public enAttendeeType AttendeeType { get; private set; }
        public DateOnly Date { get; private set; }
        public TimeOnly? CheckInTime { get; private set; }
        public TimeOnly? CheckOutTime { get; private set; }
        public AttendanceStatus Status { get; private set; }
        public Guid? ContextID { get; private set; } // اختياري: معرف الحلقة أو Session
        public enContextType? ContextType { get; private set; }
        public Guid? LeaveRequestID { get; private set; } // مرجع طلب الإذن المرتبط إن وجد
        public string? Notes { get; private set; }

        public  clsLeaveRequest? LeaveRequest { get; private set; }

        protected clsAttendance() { }

        public clsAttendance(
            Guid attendeeId,
            enAttendeeType attendeeType,
            DateOnly date,
            AttendanceStatus status,
            TimeOnly? checkInTime = null,
            TimeOnly? checkOutTime = null,
            Guid? contextId = null,
            enContextType? contextType = null,
            Guid? leaveRequestId = null,
            string? notes = null)
        {
            SetAttendee(attendeeId, attendeeType);
            SetDate(date);
            SetStatus(status);
            SetTimes(checkInTime, checkOutTime);
            SetContext(contextId, contextType);

            LeaveRequestID = leaveRequestId;
            Notes = notes;

            BaseSetCreated("System");
        }

        public void SetAttendee(Guid attendeeId, enAttendeeType attendeeType)
        {
            if (attendeeId == Guid.Empty)
                throw new DomainException(DomainExMsg.Attendance_AttendeeIDRequired);

            AttendeeID = attendeeId;
            AttendeeType = attendeeType;
        }

        public void SetDate(DateOnly date)
        {
            if (date == default)
                throw new DomainException(DomainExMsg.Attendance_DateRequired);

            Date = date;
        }

        public void SetStatus(AttendanceStatus status)
        {
            Status = status;
        }

        public void SetTimes(TimeOnly? checkIn, TimeOnly? checkOut)
        {
            if (checkIn.HasValue && checkOut.HasValue && checkOut < checkIn)
                throw new DomainException(DomainExMsg.Attendance_CheckOutBeforeCheckIn);

            CheckInTime = checkIn;
            CheckOutTime = checkOut;
        }

        public void SetExcused(Guid leaveRequestId, string? notes = null)
        {
            if (leaveRequestId == Guid.Empty)
                throw new DomainException(DomainExMsg.Attendance_LeaveRequestIDRequired);

            Status = AttendanceStatus.Excused;
            LeaveRequestID = leaveRequestId;
            if (!string.IsNullOrWhiteSpace(notes)) Notes = notes;
        }

        public void SetNotes(string? notes) { Notes = notes; }
        public void SetContextID(Guid? contextId) { ContextID = contextId; }
        public void SetContext(Guid? contextId, enContextType? contextType)
        {
            ContextID = contextId;
            ContextType = contextType;
        }
        public void SetLeaveRequestID(Guid? leaveRequestId) { LeaveRequestID = leaveRequestId; }
    }
}