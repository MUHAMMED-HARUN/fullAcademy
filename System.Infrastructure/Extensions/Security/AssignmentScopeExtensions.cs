using System.Application.Abstractions.Auth;
using System.Domain.Entities;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Extensions.Security
{
    public static class AssignmentScopeExtensions
    {
        public static IQueryable<clsAssignmentScope> ApplyAssignmentScopeReadScope(
            this IQueryable<clsAssignmentScope> query,
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
                return query.Where(scope => context.TeachingAssignments.Any(assignment =>
                    assignment.TeacherID == teacherID &&
                    assignment.Group.InstituteClass.ClassID == scope.Stage.ClassID));
            }

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(scope => context.StudentEnrollments.Any(enrollment =>
                    enrollment.StudentID == studentID &&
                    enrollment.Group.InstituteClass.ClassID == scope.Stage.ClassID));
            }

            return query.Where(_ => false);
        }

        public static IQueryable<clsAssignmentScopeinfo> ApplyAssignmentScopeInfoReadScope(
            this IQueryable<clsAssignmentScopeinfo> query,
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
                return query.Where(info => context.TeachingAssignments.Any(assignment =>
                    assignment.TeacherID == teacherID &&
                    assignment.Group.InstituteClass.ClassID == info.StageInfo.Stage.ClassID));
            }

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(info => context.StudentEnrollments.Any(enrollment =>
                    enrollment.StudentID == studentID &&
                    enrollment.Group.InstituteClass.ClassID == info.StageInfo.Stage.ClassID));
            }

            return query.Where(_ => false);
        }

        public static IQueryable<clsAssignmentNomination> ApplyAssignmentNominationReadScope(
            this IQueryable<clsAssignmentNomination> query,
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
                return query.Where(nomination =>
                    nomination.NominatedByID == teacherID ||
                    context.TeachingAssignments.Any(assignment =>
                        assignment.TeacherID == teacherID &&
                        nomination.StudentAcademicEnrollment.GroupEnrollments.Any(membership =>
                            membership.GroupID == assignment.GroupID && membership.IsActive)));
            }

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(nomination => nomination.StudentAcademicEnrollment.StudentID == studentID);
            }

            return query.Where(_ => false);
        }

        public static IQueryable<clsAssignment> ApplyAssignmentReadScope(
            this IQueryable<clsAssignment> query,
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
                return query.Where(assignment =>
                    context.TeachingAssignments.Any(teachingAssignment =>
                        teachingAssignment.TeacherID == teacherID &&
                        assignment.AssignmentNomination.StudentAcademicEnrollment.GroupEnrollments.Any(membership =>
                            membership.GroupID == teachingAssignment.GroupID && membership.IsActive)) &&
                    context.AssignmentScopeInfos.Any(info =>
                        info.AssignmentScopeID == assignment.AssignmentNomination.AssignmentScopeID) &&
                    !context.AssignmentScopeInfos.Any(info =>
                        info.AssignmentScopeID == assignment.AssignmentNomination.AssignmentScopeID &&
                        !assignment.AssignmentNomination.StudentAcademicEnrollment.Items.Any(item =>
                            item.StageInfoID == info.StageInfoID)));
            }

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(assignment =>
                    assignment.AssignmentNomination.StudentAcademicEnrollment.StudentID == studentID &&
                    context.AssignmentScopeInfos.Any(info =>
                        info.AssignmentScopeID == assignment.AssignmentNomination.AssignmentScopeID) &&
                    !context.AssignmentScopeInfos.Any(info =>
                        info.AssignmentScopeID == assignment.AssignmentNomination.AssignmentScopeID &&
                        !assignment.AssignmentNomination.StudentAcademicEnrollment.Items.Any(item =>
                            item.StageInfoID == info.StageInfoID)));
            }

            return query.Where(_ => false);
        }

        public static IQueryable<clsResult> ApplyResultReadScope(
            this IQueryable<clsResult> query,
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
                return query.Where(result =>
                    context.TeachingAssignments.Any(teachingAssignment =>
                        teachingAssignment.TeacherID == teacherID &&
                        result.Assignment.AssignmentNomination.StudentAcademicEnrollment.GroupEnrollments.Any(membership =>
                            membership.GroupID == teachingAssignment.GroupID && membership.IsActive)) &&
                    result.AssignmentScopeInfo.AssignmentScopeID ==
                        result.Assignment.AssignmentNomination.AssignmentScopeID &&
                    result.Assignment.AssignmentNomination.StudentAcademicEnrollment.Items.Any(item =>
                        item.StageInfoID == result.AssignmentScopeInfo.StageInfoID));
            }

            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid studentID && studentID != Guid.Empty)
            {
                return query.Where(result =>
                    result.Assignment.AssignmentNomination.StudentAcademicEnrollment.StudentID == studentID &&
                    result.AssignmentScopeInfo.AssignmentScopeID ==
                        result.Assignment.AssignmentNomination.AssignmentScopeID &&
                    result.Assignment.AssignmentNomination.StudentAcademicEnrollment.Items.Any(item =>
                        item.StageInfoID == result.AssignmentScopeInfo.StageInfoID));
            }

            return query.Where(_ => false);
        }
    }
}
