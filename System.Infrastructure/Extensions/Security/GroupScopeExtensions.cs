using System.Application.Abstractions.Auth;
using System.Domain.Entities;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Extensions.Security
{
    public static class GroupScopeExtensions
    {
        public static IQueryable<clsGroup> ApplyGroupReadScope(
            this IQueryable<clsGroup> query,
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
                return query.Where(group => context.TeachingAssignments.Any(assignment =>
                    assignment.TeacherID == teacherID && assignment.GroupID == group.Id && assignment.IsActive));
            }

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(group => context.StudentEnrollments.Any(enrollment =>
                    enrollment.StudentID == studentID && enrollment.GroupID == group.Id && enrollment.IsActive));
            }

            return query.Where(_ => false);
        }
    }
}
