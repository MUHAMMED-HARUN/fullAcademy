using System.Application.Abstractions.Auth;
using System.Domain.Entities.Progress;
using System.Domain.Entities.Enrollment;
using System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace System.Infrastructure.Extensions.Security
{
    public static class StageInfoScopeExtensions
    {
        public static IQueryable<clsStageInfo> ApplyStageInfoReadScope(
            this IQueryable<clsStageInfo> query,
            ICurrentUserService currentUser,
            AppDbContext context)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);

            // StageInfo is academic content (stage + subject matter), not student-specific data.
            // Admin must see ALL StageInfo records regardless of enrollments.
            if (currentUser.IsAdmin)
                return query;

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                var academicEnrollments = context.StudentAcademicEnrollments.AsNoTracking()
                    .ApplyStudentAcademicEnrollmentReadScope(currentUser, context);

                return query.Where(info => academicEnrollments.Any(enrollment =>
                    enrollment.Items.Any(item => item.StageInfoID == info.Id)));
            }

            if (string.Equals(currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid teacherID && teacherID != Guid.Empty)
            {
                // Teacher sees StageInfo for stages in classes they teach.
                // Path: TeachingAssignment → Group → InstituteClass → Class → Stage → StageInfo
                var taughtClassIDs = context.TeachingAssignments.AsNoTracking()
                    .Where(assignment => assignment.TeacherID == teacherID && assignment.IsActive)
                    .Select(assignment => assignment.Group.InstituteClass.ClassID);

                var taughtStageIDs = context.Stages.AsNoTracking()
                    .Where(stage => taughtClassIDs.Contains(stage.ClassID))
                    .Select(stage => stage.Id);

                return query.Where(info => taughtStageIDs.Contains(info.StageID));
            }

            // Fallback: no access for unrecognized roles.
            return query.Where(_ => false);
        }

    }
}
