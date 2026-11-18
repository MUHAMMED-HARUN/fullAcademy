using System.Application.Abstractions.Auth;
using System.Domain.Entities;
using System.Domain.Entities.Attendance;
using System.Domain.Enums.Attendance;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Extensions.Security
{
    public static class AttendanceScopeExtensions
    {
        public static IQueryable<clsStudentEnrollment> ApplyAttendanceWriteScope(
            this IQueryable<clsStudentEnrollment> query,
            ICurrentUserService currentUser,
            AppDbContext context,
            DateOnly attendanceDate)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);
            if (currentUser.IsAdmin)
                return query;

            if (string.Equals(currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
            {
                return query.Where(enrollment => context.TeachingAssignments.Any(assignment =>
                    assignment.TeacherID == teacherID &&
                    assignment.GroupID == enrollment.GroupID &&
                    assignment.IsActive && assignment.Date <= attendanceDate));
            }

            return query.Where(_ => false);
        }

        public static IQueryable<clsAttendance> ApplyAttendanceReadScope(
            this IQueryable<clsAttendance> query,
            ICurrentUserService currentUser,
            AppDbContext context)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);

            if (currentUser.IsAdmin)
                return query;

            if (string.Equals(currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
            {
                return query.Where(attendance =>
                    attendance.AttendeeType == enAttendeeType.Student &&
                    attendance.ContextType == enContextType.Group &&
                    attendance.ContextID.HasValue &&
                    context.StudentEnrollments.Any(enrollment =>
                        enrollment.Id == attendance.AttendeeID &&
                        enrollment.GroupID == attendance.ContextID.Value &&
                        enrollment.IsActive && enrollment.Date <= attendance.Date) &&
                    context.TeachingAssignments.Any(assignment =>
                        assignment.TeacherID == teacherID &&
                        assignment.GroupID == attendance.ContextID.Value &&
                        assignment.IsActive && assignment.Date <= attendance.Date));
            }

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(attendance =>
                    attendance.AttendeeType == enAttendeeType.Student &&
                    attendance.ContextType == enContextType.Group &&
                    attendance.ContextID.HasValue &&
                    context.StudentEnrollments.Any(enrollment =>
                        enrollment.Id == attendance.AttendeeID &&
                        enrollment.StudentID == studentID &&
                        enrollment.GroupID == attendance.ContextID.Value &&
                        enrollment.IsActive && enrollment.Date <= attendance.Date));
            }

            return query.Where(_ => false);
        }
    }
}
