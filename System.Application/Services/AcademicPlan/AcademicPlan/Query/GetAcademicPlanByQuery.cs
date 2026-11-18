using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.AcademicPlan.AcademicPlan.DTO;

namespace System.Application.Services.AcademicPlan.AcademicPlan.Query
{
    public class GetAcademicPlanByQuery : IRequest<TResult<AcademicPlanInfo>>, IQuery
    {
        public string AcademicPlanID { get; set; }
    }
}
