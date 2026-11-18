using System.Application.Abstractions.Auth;
using System.Domain.Entities;
using System.Domain.Entities.Enrollment;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Extensions.Security
{
    public static class StudentAcademicEnrollmentScopeExtensions
    {
        public static IQueryable<clsStudentAcademicEnrollment> ApplyStudentAcademicEnrollmentReadScope(
            this IQueryable<clsStudentAcademicEnrollment> query,
            ICurrentUserService currentUser,
            AppDbContext context)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);
            if (currentUser.IsAdmin)
                return query;

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
                return query.Where(enrollment => enrollment.StudentID == studentID);

            if (string.Equals(currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
            {
                return query.Where(enrollment => context.StudentEnrollments.Any(membership =>
                    membership.StudentAcademicEnrollmentID == enrollment.Id &&
                    membership.IsActive &&
                    context.TeachingAssignments.Any(assignment =>
                        assignment.GroupID == membership.GroupID &&
                        assignment.TeacherID == teacherID &&
                        assignment.IsActive)));
            }

            return query.Where(_ => false);
        }
    }
}
