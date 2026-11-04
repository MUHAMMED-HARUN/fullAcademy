using System.Application.Abstractions.Auth;
using System.Domain.Entities.Progress;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Extensions.Security
{
    public static class SessionScopeExtensions
    {
        public static IQueryable<clsSession> ApplySessionReadScope(
            this IQueryable<clsSession> query,
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
                return query.Where(session => context.TeachingAssignments.Any(assignment =>
                    assignment.TeacherID == teacherID &&
                    (assignment.SubjectID == session.Id || assignment.SubjectID == session.SessionID)));
            }

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(session => context.StudentEnrollments.Any(enrollment =>
                    enrollment.StudentID == studentID &&
                    context.TeachingAssignments.Any(assignment =>
                        assignment.GroupID == enrollment.GroupID &&
                        (assignment.SubjectID == session.Id || assignment.SubjectID == session.SessionID))));
            }

            return query.Where(_ => false);
        }
    }
}
