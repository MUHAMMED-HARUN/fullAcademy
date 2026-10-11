
using System.Domain.Entities.BaseClass;
using System.Domain.Enums.Attendance;
using System.Domain.Exceptions;

namespace System.Domain.Entities.Attendance
{
    public class clsLeaveRequest : clsBaseClass
    {
        public Guid EntityID{ get; private set; }
        public enAttendeeType AttendeeType { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        public string? Reason { get; private set; }
        public LeaveStatus Status { get; private set; }
        public Guid? ApprovedByID { get; private set; }
        public string? RejectionReason { get; private set; }
            
        protected clsLeaveRequest() { }

        public clsLeaveRequest(
            Guid entityID,
            enAttendeeType applicantType,
            LeaveType leaveType,
            DateOnly startDate,
            DateOnly endDate,
            LeaveStatus leaveStatus,
            string reason,
            Guid approvedBy,
            string? rejectionReason)
        {
            SetEntityID(entityID, applicantType);
            SetLeavType(leaveType);
            SetDates(startDate, endDate);
            SetReason(reason);

            SetLeaveStatus(LeaveStatus.Pending,approvedBy,rejectionReason);

            BaseSetCreated("System");
        }
        public void SetEntityID(Guid entityID, enAttendeeType attendeeType)
        {
            if (entityID == Guid.Empty)
                throw new DomainException(DomainExMsg.LeaveRequest_EntityIDRequired);
            EntityID = entityID;
            AttendeeType = attendeeType;
        }
        public void SetDates(DateOnly startDate, DateOnly endDate)
        {
            if (startDate > endDate)
                throw new DomainException(DomainExMsg.LeaveRequest_StartDateAfterEndDate);
            StartDate = startDate;
            EndDate = endDate;
        }
        public void SetReason(string reason)
        {
            Reason = reason;
        }

        public void SetLeaveStatus(LeaveStatus status, Guid? approvedByPersonID = null, string? rejectionReason = null)
        {
            Status = status;
            ApprovedByID = approvedByPersonID;
            RejectionReason = rejectionReason;
        }
        public void SetLeavType (LeaveType leaveType)
        {
            LeaveType = leaveType;
        }
        public void SetApprovedByID(Guid? approverID)
        {
            ApprovedByID = approverID;
        }
        public void SetRejectionReason(string? rejectionReason) 
        {
            RejectionReason = rejectionReason;
        }

    }
}