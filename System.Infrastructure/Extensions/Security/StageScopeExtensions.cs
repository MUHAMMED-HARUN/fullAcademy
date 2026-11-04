using System.Application.Abstractions.Auth;
using System.Domain.Entities.Progress;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Extensions.Security
{
    public static class StageScopeExtensions
    {
        public static IQueryable<clsStage> ApplyStageReadScope(
            this IQueryable<clsStage> query,
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
                return query.Where(stage => context.TeachingAssignments.Any(assignment =>
                    assignment.TeacherID == teacherID &&
                    assignment.Group.InstituteClass.ClassID == stage.ClassID));
            }

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(stage => context.StudentEnrollments.Any(enrollment =>
                    enrollment.StudentID == studentID &&
                    enrollment.Group.InstituteClass.ClassID == stage.ClassID));
            }

            return query.Where(_ => false);
        }
    }
}
