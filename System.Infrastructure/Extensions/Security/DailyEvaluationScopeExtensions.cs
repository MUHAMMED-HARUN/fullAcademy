using System.Application.Abstractions.Auth;
using System.Domain.Entities;
using System.Domain.Entities.Enrollment;
using System.Domain.Enums.Attendance;
using System.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace System.Infrastructure.Extensions.Security
{
    public static class DailyEvaluationScopeExtensions
    {
        public static IQueryable<clsDailyEvaluation> ApplyDailyEvaluationReadScope(
            this IQueryable<clsDailyEvaluation> query,
            ICurrentUserService currentUser,
            AppDbContext context)
        {
            var academicEnrollments = context.StudentAcademicEnrollments.AsNoTracking()
                .ApplyStudentAcademicEnrollmentReadScope(currentUser, context);

            return query.Where(evaluation => academicEnrollments.Any(enrollment =>
                enrollment.Id == evaluation.StudentAcademicEnrollmentID &&
                enrollment.Items.Any(item => item.StageInfoID == evaluation.StageInfoID)));
        }
    }
}
