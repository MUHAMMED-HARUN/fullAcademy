using System.Application.Abstractions.Auth;
using System.Domain.Entities.AcademicPlan;
using System.Domain.Enums.AcademicPlan;

namespace System.Infrastructure.Extensions.Security
{
    public static class AcademicPlanScopeExtensions
    {
        public static IQueryable<clsAcademicPlan> ApplyAcademicPlanReadScope(
            this IQueryable<clsAcademicPlan> query,
            ICurrentUserService currentUser)
        {
            if (!currentUser.IsAuthenticated)
                return query.Where(_ => false);

            if (currentUser.IsAdmin)
                return query;

            // Teachers and students browse only published plans; enrollment-based visibility is added with StudentAcademicEnrollment.
            if ((string.Equals(currentUser.EntityType, "Teacher", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(currentUser.EntityType, "Student", StringComparison.OrdinalIgnoreCase)) &&
                currentUser.EntityGuid is Guid entityID && entityID != Guid.Empty)
            {
                return query.Where(plan => plan.Status == enAcademicPlanStatus.Active);
            }

            return query.Where(_ => false);
        }
    }
}
