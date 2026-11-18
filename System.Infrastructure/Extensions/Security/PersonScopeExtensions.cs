using System.Application.Abstractions.Auth;
using System.Domain.Entities;
using System.Domain.Entities.Person;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Extensions.Security
{
    public static class PersonScopeExtensions
    {
        public static IQueryable<clsStudent> ApplyStudentReadScope(
            this IQueryable<clsStudent> query,
            ICurrentUserService currentUser,
            AppDbContext context)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);

            if (currentUser.IsAdmin)
                return query;

            if (TryGetTeacherID(currentUser, out var teacherID))
            {
                var groupIDs = AuthorizedGroupIDs(context, teacherID);
                return query.Where(student => context.StudentEnrollments.Any(enrollment =>
                    enrollment.StudentID == student.Id && groupIDs.Contains(enrollment.GroupID)));
            }

            if (TryGetStudentID(currentUser, out var studentID))
                return query.Where(student => student.Id == studentID);

            return query.Where(_ => false);
        }

        public static IQueryable<clsTeacher> ApplyTeacherReadScope(
            this IQueryable<clsTeacher> query,
            ICurrentUserService currentUser)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);

            if (currentUser.IsAdmin)
                return query;

            if (TryGetTeacherID(currentUser, out var teacherID))
                return query.Where(teacher => teacher.Id == teacherID);

            return query.Where(_ => false);
        }

        public static IQueryable<clsPerson> ApplyPersonReadScope(
            this IQueryable<clsPerson> query,
            ICurrentUserService currentUser,
            AppDbContext context)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);

            if (currentUser.IsAdmin)
                return query;

            if (TryGetTeacherID(currentUser, out var teacherID))
            {
                var groupIDs = AuthorizedGroupIDs(context, teacherID);
                var ownPersonIDs = context.Set<clsTeacher>()
                    .Where(teacher => teacher.Id == teacherID)
                    .Select(teacher => teacher.PersonID);
                var studentPersonIDs = context.Students
                    .Where(student => context.StudentEnrollments.Any(enrollment =>
                        enrollment.StudentID == student.Id && groupIDs.Contains(enrollment.GroupID)))
                    .Select(student => student.PersonID);

                return query.Where(person =>
                    ownPersonIDs.Contains(person.Id) || studentPersonIDs.Contains(person.Id));
            }

            if (TryGetStudentID(currentUser, out var studentID))
            {
                var ownPersonIDs = context.Students
                    .Where(student => student.Id == studentID)
                    .Select(student => student.PersonID);

                return query.Where(person => ownPersonIDs.Contains(person.Id));
            }

            return query.Where(_ => false);
        }

        private static IQueryable<Guid> AuthorizedGroupIDs(AppDbContext context, Guid teacherID) =>
            context.TeachingAssignments
                .Where(assignment => assignment.TeacherID == teacherID)
                .Select(assignment => assignment.GroupID);

        private static bool TryGetTeacherID(ICurrentUserService currentUser, out Guid teacherID)
        {
            teacherID = Guid.Empty;
            if (string.Equals(currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid entityID && entityID != Guid.Empty)
            {
                teacherID = entityID;
                return true;
            }
            return false;
        }

        private static bool TryGetStudentID(ICurrentUserService currentUser, out Guid studentID)
        {
            studentID = Guid.Empty;
            if (string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase) &&
                currentUser.EntityGuid is Guid entityID && entityID != Guid.Empty)
            {
                studentID = entityID;
                return true;
            }
            return false;
        }
    }
}
