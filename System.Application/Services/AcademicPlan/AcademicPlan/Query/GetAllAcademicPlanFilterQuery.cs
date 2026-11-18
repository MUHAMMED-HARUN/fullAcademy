using System.Application.Common.Models;
using System.Application.Services.AcademicPlan.AcademicPlan.DTO;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Query
{
    public sealed class GetAllAcademicPlanFilterQuery : BaseFilterQuery<AcademicPlanLookupInfo>
    {
        public GetAllAcademicPlanFilterQuery()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
