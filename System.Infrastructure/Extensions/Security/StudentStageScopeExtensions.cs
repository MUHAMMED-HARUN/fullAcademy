using System.Application.Abstractions.Auth;
using System.Domain.Entities;
using System.Domain.Entities.Enrollment;
using System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace System.Infrastructure.Extensions.Security
{
    public static class StudentStageScopeExtensions
    {
        public static IQueryable<clsStudentStage> ApplyStudentStageReadScope(
            this IQueryable<clsStudentStage> query,
            ICurrentUserService currentUser,
            AppDbContext context)
        {
            var academicEnrollments = context.StudentAcademicEnrollments.AsNoTracking()
                .ApplyStudentAcademicEnrollmentReadScope(currentUser, context);

            return query.Where(studentStage => academicEnrollments.Any(enrollment =>
                enrollment.Id == studentStage.StudentAcademicEnrollmentID &&
                enrollment.Items.Any(item => item.StageID == studentStage.StageID)));
        }
    }
}
