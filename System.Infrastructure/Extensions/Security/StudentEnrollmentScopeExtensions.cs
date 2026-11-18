using System.Application.Abstractions.Auth;
using System.Domain.Entities;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Extensions.Security
{
    public static class StudentEnrollmentScopeExtensions
    {
        public static IQueryable<clsStudentEnrollment> ApplyStudentEnrollmentReadScope(
            this IQueryable<clsStudentEnrollment> query,
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
                var authorizedGroupIDs = context.TeachingAssignments
                    .Where(assignment => assignment.TeacherID == teacherID)
                    .Select(assignment => assignment.GroupID);

                return query.Where(enrollment =>
                    authorizedGroupIDs.Contains(enrollment.GroupID));
            }

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(enrollment => enrollment.StudentID == studentID);
            }

            return query.Where(_ => false);
        }
    }
}