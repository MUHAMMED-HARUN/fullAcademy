using System.Application.Abstractions.Auth;
using System.Domain.Entities;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Extensions.Security
{
    public static class TeachingAssignmentScopeExtensions
    {
        public static IQueryable<clsTeachingAssignment> ApplyTeachingAssignmentReadScope(
            this IQueryable<clsTeachingAssignment> query,
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
                return query.Where(assignment => assignment.TeacherID == teacherID);
            }

            return query.Where(_ => false);
        }
    }
}